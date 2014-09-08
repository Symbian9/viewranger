using Liath.ViewRanger.Exceptions;
using Liath.ViewRanger.Responses;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Liath.ViewRanger.RequestBuilders
{
    public abstract class RequestBase
    {
        private static ILog s_log = LogManager.GetLogger(typeof(RequestBase));

        protected const string RequestFormat = "xml";
        protected string BaseAddress { get; set; }
        protected string Key { get; set; }

        // Keys
        public const string KeyKey = "key";
        public const string ServiceKey = "service";
        public const string UsernameKey = "username";
        public const string PinKey = "pin";
        public const string FormatKey = "format";

        protected RequestBase(string applicationKey)
        {
            if (applicationKey == null) throw new ArgumentNullException("applicationKey");

            this.BaseAddress = @"http://api.viewranger.com/public/v1/";
        }

        public virtual XDocument MakeRequest(string service, string username, string pin)
        {
            var url = this.CreateUrl(new RequestParameter(KeyKey, Key),
                new RequestParameter(ServiceKey, service),
                new RequestParameter(UsernameKey, username),
                new RequestParameter(PinKey, pin),
                new RequestParameter(FormatKey, RequestFormat));

            var document = this.DownloadXml(url);
            return document;

            // See http://www.viewranger.com/developers/documentation/
            // Here's a sample url - http://api.viewranger.com/public/v1/?key={API-KEY}&service=getLastBBPosition&username={BB-USERNAME}&pin={BB-PIN}&format={FORMAT}

            // example response
            //<?xml version="1.0" encoding="UTF-8"?>
            //    <VIEWRANGER>
            //        <LOCATION>
            //            <LATITUDE>52.2087</LATITUDE>
            //            <LONGITUDE>0.124226</LONGITUDE>
            //            <DATE>2011-09-22 09:52:58</DATE>
            //            <ALTITUDE>10</ALTITUDE>
            //            <SPEED>1</SPEED>
            //            <HEADING>270</HEADING>
            //        </LOCATION>
            //    </VIEWRANGER>
            //    See more at: http://www.viewranger.com/developers/documentation/#sthash.wi7BbDza.dpuf
        }

        public virtual string CreateUrl(params RequestParameter[] parameters)
        {
            return string.Concat(this.BaseAddress, "?", string.Join("&", parameters.Select(x => string.Join(x.Key, "=", x.Value)).ToArray()));
        }

        public virtual XDocument DownloadXml(string url)
        {
            return XDocument.Load(url);
        }

        public Location CreateLocationFromXml(XElement locationElement)
        {            
            //        <LOCATION>
            //            <LATITUDE>52.2087</LATITUDE>
            //            <LONGITUDE>0.124226</LONGITUDE>
            //            <DATE>2011-09-22 09:52:58</DATE>
            //            <ALTITUDE>10</ALTITUDE>
            //            <SPEED>1</SPEED>
            //            <HEADING>270</HEADING>
            //        </LOCATION>

            return new Location
            {
                Altitude = this.GetDecimalValue(locationElement, "ALTITUDE"),
                Heading = this.GetDecimalValue(locationElement, "HEADING"),
                Latitude = this.GetDecimalValue(locationElement, "LATITUDE"),
                Longitude = this.GetDecimalValue(locationElement, "LONGITUDE"),
                Speed = this.GetDecimalValue(locationElement, "SPEED"),
                Date = this.GetDateTimeValue(locationElement, "DATE")
            };
        }

        private decimal? GetDecimalValue(XElement element, string key)
        {            
            return this.GetValue<decimal>(element, key, singleMatch =>
                {
                    decimal value;
                    if (decimal.TryParse(singleMatch.Value, out value))
                    {
                        return value;
                    }
                    else
                    {
                        s_log.ErrorFormat("The value '{0}' in the element {1} could not be parsed to a decimal", singleMatch.Value, key);
                        throw new UnexpectedResponseException("The value '{0}' in the element {1} could not be parsed to a decimal", singleMatch.Value, key);
                    }
                });
        }

        private DateTime? GetDateTimeValue(XElement element, string key)
        {
            return this.GetValue<DateTime>(element, key, singleMatch =>
            {
                DateTime value;
                if (DateTime.TryParse(singleMatch.Value, out value))
                {
                    return value;
                }
                else
                {
                    s_log.ErrorFormat("The value '{0}' in the element {1} could not be parsed to a DateTime", singleMatch.Value, key);
                    throw new UnexpectedResponseException("The value '{0}' in the element {1} could not be parsed to a DateTime", singleMatch.Value, key);
                }
            });
        }

        private T? GetValue<T>(XElement element,  string key, Func<XElement, T> innerFunction) where T : struct
        {
            var matches = element.Descendants(key);
            if(matches.Count() == 0)
            {
                return null;
            }
            else if(matches.Count() == 1)
            {
                return innerFunction(matches.Single());
            }
            else // > 1
            {
                s_log.ErrorFormat("The LOCATION element had {0} {1} elements", matches.Count(), key);
                throw new UnexpectedResponseException("The LOCATION element had {0} {1} elements", matches.Count(), key);
            }
        }
    }
}