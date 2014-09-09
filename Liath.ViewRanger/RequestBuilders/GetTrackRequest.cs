using Liath.ViewRanger.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liath.ViewRanger.RequestBuilders
{
    public class GetTrackRequest : RequestBase, IGetTrackRequest
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int LimitValue { get; set; }

        public GetTrackRequest(string apiKey)
            : base(apiKey)
        {
            this.FromDate = DateTime.MinValue;
            this.ToDate = DateTime.MaxValue;
        }

        public override string Service
        {
            get { return "getBBPositions"; }
        }

        public Track Request()
        {
            throw new NotImplementedException();
        }        


        //http://api.viewranger.com/public/v1/?key={API-KEY}&service=getBBPositions&username={BB-USERNAME}&pin={BB-PIN}&date_from={DATE-FROM}&date_until={DATE-UNTIL}&limit={LIMIT}&format={FORMAT}#sthash.kKhQV9EG.dpuf

        //<?xml version="1.0" encoding="UTF-8"?> <VIEWRANGER> <LOCATION> <LATITUDE>51.4609</LATITUDE> <LONGITUDE>-2.58858</LONGITUDE> <DATE>2011-09-21 15:11:05</DATE> <ALTITUDE>0</ALTITUDE> <SPEED>0</SPEED> <HEADING>188</HEADING> </LOCATION> <LOCATION> <LATITUDE>51.4609</LATITUDE> <LONGITUDE>-2.58854</LONGITUDE> <DATE>2011-09-21 15:07:05</DATE> <ALTITUDE>0</ALTITUDE> <SPEED>0</SPEED> <HEADING>8</HEADING> </LOCATION> <LOCATION> <LATITUDE>51.4609</LATITUDE> <LONGITUDE>-2.58858</LONGITUDE> <DATE>2011-09-21 15:03:05</DATE> <ALTITUDE>0</ALTITUDE> <SPEED>0</SPEED> <HEADING>205</HEADING> </LOCATION> </VIEWRANGER> - See more at: http://www.viewranger.com/developers/documentation/#sthash.kKhQV9EG.dpuf


        public IGetTrackRequest From(DateTime from)
        {
            this.FromDate = from;
            return this;
        }

        public IGetTrackRequest To(DateTime to)
        {
            this.ToDate = to;
            return this;
        }

        public IGetTrackRequest Limit(int limit)
        {
            this.LimitValue = limit;
            return this;
        }

        public IGetTrackRequest ForUser(string username, string pin)
        {
            if (username == null) throw new ArgumentNullException("username");
            if (pin == null) throw new ArgumentNullException("pin");

            this.Username = username;
            this.Pin = pin;

            return this;
        }
    }
}
