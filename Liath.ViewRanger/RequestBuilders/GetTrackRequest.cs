using Liath.ViewRanger.Responses;
using log4net;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Liath.ViewRanger.RequestBuilders
{
    public class GetTrackRequest : RequestBase, IGetTrackRequest
    {
        private static ILog s_log = LogManager.GetLogger(typeof(GetTrackRequest));

        public const string DateTimeFormatString = "yyyy-MM-dd HH:mm:ss";
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int? LimitValue { get; set; }

        public const string FromKey = "date_from";
        public const string ToKey = "date_until";
        public const string LimitKey = "limit";
        public const int MaxMatchSize = 500;

        public GetTrackRequest(string apiKey, string baseAddress)
            : base(apiKey, baseAddress)
        {
            this.FromDate = DateTime.MinValue;
            this.ToDate = DateTime.MaxValue;
            this.LimitValue = 50;
        }

        public override string Service
        {
            get { return "getBBPositions"; }
        }

        public Track Request()
        {
            return this.HandleExceptions(s_log, () =>
                {
                    string batchSize = this.LimitValue.HasValue ? this.LimitValue.ToString() : MaxMatchSize.ToString();
                    s_log.DebugFormat("Setting BatchSize to {0}", batchSize);
                    var allLocations = new List<Location>();
                    IEnumerable<Location> thisBatch;
                    do
                    {
                        // Update the toDate filter to exclude any results we've already downloaded
                        var toDateFilter = allLocations.Any()
                            ? allLocations.Min(l => l.Date).Value.AddSeconds(-1)
                            : this.ToDate;
                        
                        s_log.DebugFormat("Downloading a maximum of {0} results between {1} and {2}",
                            batchSize,
                            this.FromDate.ToString(DateTimeFormatString),
                            toDateFilter.ToString(DateTimeFormatString));

                        // download the new xml
                        var xml = this.MakeRequest(new RequestParameter(FromKey, this.FromDate.ToString(DateTimeFormatString)),
                            new RequestParameter(ToKey, toDateFilter.ToString(DateTimeFormatString)),
                            new RequestParameter(LimitKey, batchSize));

                        // parse and add
                        thisBatch = this.ParseLocationsFromXml(xml);
                        allLocations.AddRange(thisBatch);
                        s_log.DebugFormat("{0} new locations downloaded, {1} downloaded in total", thisBatch.Count(), allLocations.Count);

                        // if we're doing a limitless query and if the result count was equal to the batch size then go again
                    } while (!this.LimitValue.HasValue && thisBatch.Count() == MaxMatchSize);

                    var sortedLocations = allLocations.OrderBy(l => l.Date);
                    DateTime? start;
                    DateTime? end;
                    TimeSpan? duration;
                    decimal? height;
                    decimal? distance;
                    this.CalculateTrackDateProperties(sortedLocations, out start, out end, out duration);
                    this.CalculateGeographicProperties(sortedLocations, out distance, out height);
                    return new Track
                    { 
                        Locations =  sortedLocations,
                        StartTime = start,
                        EndTime = end,
                        Duration = duration,
                        TotalDistance = distance,
                        TotalHeightGain = height
                    };
                });
        }

        /// <summary>
        /// Calcualtes the distance and height gain of the track
        /// </summary>
        private void CalculateGeographicProperties(IOrderedEnumerable<Location> locations, out decimal? distance, out decimal? height)
        {
            // There may be some room for improvement here - rather than iterating two sublists perhaps it's better to iterate everything once?

            s_log.DebugFormat("Calculating geographic properties of track with {0} locations", locations.Count());
            var valuesWithCoords = locations.Where(l => l.Latitude.HasValue && l.Longitude.HasValue);
            s_log.DebugFormat("{0} locations have Lat/Lng information", valuesWithCoords.Count());
            if(valuesWithCoords.Any())
            {
                var edges = new List<double>();
                for(int i = 1; i < valuesWithCoords.Count(); i++)
                {
                    var startNode = valuesWithCoords.ElementAt(i - 1);
                    var endNode = valuesWithCoords.ElementAt(i);
                    edges.Add(new GeoCoordinate((double)startNode.Latitude.Value, (double)startNode.Longitude.Value)
                        .GetDistanceTo(new GeoCoordinate((double)endNode.Latitude.Value, (double)endNode.Longitude.Value)));
                }

                distance = (decimal)edges.Sum();
                s_log.DebugFormat("Total distance of {0} calculated", distance);
            }
            else
            {
                s_log.Debug("No locations have coordinates, TotalDistance will be null");
                distance = null;
            }

            var valuesWithHeights = locations.Where(l => l.Altitude.HasValue);
            height = 0;
            if(valuesWithHeights.Any())
            {
                for(int i = 1; i < valuesWithHeights.Count(); i++)
                {
                    var previousLocation = valuesWithHeights.ElementAt(i - 1);
                    var thisLocation = valuesWithHeights.ElementAt(i);
                    var difference = previousLocation.Altitude.Value - thisLocation.Altitude.Value;
                    if(difference > 0) // this is a height gain - only measure if it's greater than zero
                    {
                        height += difference;
                    }
                }
            }
            else
            {
                s_log.DebugFormat("There were no location with heights");
                height = null;
            }            
        }

        /// <summary>
        /// Calculates the various time related properties of a Track
        /// </summary>
        private void CalculateTrackDateProperties(IOrderedEnumerable<Location> locations, out DateTime? start, out DateTime? end, out TimeSpan? duration)
        {            
            var locationsWithDates = locations.Where(x => x.Date != null);
            s_log.DebugFormat("Calculating time properties of track with {0} locations {1} of those have Date data", locations.Count(), locationsWithDates.Count());

            if(locations != null && locations.Any() && locationsWithDates.Any())
            {
                start = locationsWithDates.Min(x => x.Date);
                end = locationsWithDates.Max(x => x.Date);

                if (start.HasValue && end.HasValue)
                {
                    duration = end.Value.Subtract(start.Value);
                }
                else
                {
                    s_log.Debug("Either the Start End Time of track was not found, the duration will be null");
                    duration = null;
                }
            }
            else
            {
                s_log.Debug("No locations were found. StartTime, EndTime and Duration will be null");
                start = null;
                end = null;
                duration = null;
            }
        }

        private IEnumerable<Location> ParseLocationsFromXml(XDocument xml)
        {
            var locationElements = xml.Descendants("LOCATION");
            s_log.DebugFormat("{0} location elements found", locationElements.Count());
            return locationElements.Select(le => this.CreateLocationFromXml(le)).ToArray(); // force evaluation now to get any errors
        }


        //http://api.viewranger.com/public/v1/?key={API-KEY}&service=getBBPositions&username={BB-USERNAME}&pin={BB-PIN}&date_from={DATE-FROM}&date_until={DATE-UNTIL}&limit={LIMIT}&format={FORMAT}#sthash.kKhQV9EG.dpuf

        //<?xml version="1.0" encoding="UTF-8"?> <VIEWRANGER> <LOCATION> <LATITUDE>51.4609</LATITUDE> <LONGITUDE>-2.58858</LONGITUDE> <DATE>2011-09-21 15:11:05</DATE> <ALTITUDE>0</ALTITUDE> <SPEED>0</SPEED> <HEADING>188</HEADING> </LOCATION> <LOCATION> <LATITUDE>51.4609</LATITUDE> <LONGITUDE>-2.58854</LONGITUDE> <DATE>2011-09-21 15:07:05</DATE> <ALTITUDE>0</ALTITUDE> <SPEED>0</SPEED> <HEADING>8</HEADING> </LOCATION> <LOCATION> <LATITUDE>51.4609</LATITUDE> <LONGITUDE>-2.58858</LONGITUDE> <DATE>2011-09-21 15:03:05</DATE> <ALTITUDE>0</ALTITUDE> <SPEED>0</SPEED> <HEADING>205</HEADING> </LOCATION> </VIEWRANGER> - See more at: http://www.viewranger.com/developers/documentation/#sthash.kKhQV9EG.dpuf


        public IGetTrackRequest From(DateTime from)
        {
            return this.HandleExceptions(s_log, () =>
               {
                   this.FromDate = from;
                   return this;
               });
        }

        public IGetTrackRequest To(DateTime to)
        {
            return this.HandleExceptions(s_log, () =>
               {
                   this.ToDate = to;
                   return this;
               });
        }

        public IGetTrackRequest Limit(int limit)
        {
            return this.HandleExceptions(s_log, () =>
               {
                   this.LimitValue = limit;
                   return this;
               });
        }

        public IGetTrackRequest NoLimit()
        {
            return this.HandleExceptions(s_log, () =>
                {
                    this.LimitValue = null; // removes the max limit of batches
                    return this;
                });
        }

        public IGetTrackRequest ForUser(string username, string pin)
        {
            if (username == null) throw new ArgumentNullException("username");
            if (pin == null) throw new ArgumentNullException("pin");

            return this.HandleExceptions(s_log, () =>
               {
                   this.Username = username;
                   this.Pin = pin;

                   return this;
               });
        }


        public IGetTrackRequest ForToday()
        {
            return this.HandleExceptions(s_log, () =>
               {
                   // If we need to make this more robust we could insert a TimeManager dependency
                   var today = DateTime.Now.Date;
                   this.FromDate = today; // midnight just gone
                   this.ToDate = today.AddDays(1); // midnight coming up

                   return this;
               });
        }


        public IGetTrackRequest ForDay(DateTime date)
        {
            return this.HandleExceptions(s_log, () =>
                {
                    this.FromDate = date.Date;
                    this.ToDate = this.FromDate.AddDays(1);

                    return this;
                });
        }
    }
}