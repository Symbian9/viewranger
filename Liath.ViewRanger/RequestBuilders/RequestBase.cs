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
        public virtual string BaseAddress { get; set; }
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
            this.Key = applicationKey;
        }

        public virtual XDocument MakeRequest(string service, string username, string pin)
        {
            var url = this.CreateUrl(new RequestParameter(KeyKey, Key),
                new RequestParameter(ServiceKey, service),
                new RequestParameter(UsernameKey, username),
                new RequestParameter(PinKey, pin),
                new RequestParameter(FormatKey, RequestFormat));

            var document = this.DownloadXml(url);
            this.HandleErrors(document);
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

        private void HandleErrors(XDocument document)
        {          
            var errorElements = document.Descendants("ERROR");
            if(errorElements.Count() > 0)
            {
                s_log.ErrorFormat("{0} errors detected", errorElements.Count());
                var firstErrorElement = errorElements.First();
                var ex = this.CreateExceptionFromError(firstErrorElement);
                throw ex;
            }                   
        }

        private Exception CreateExceptionFromError(XElement errorElement)
        {
            //<VIEWRANGER>
            //  <ERROR>
            //    <MESSAGE>Invalid API key</MESSAGE>
            //    <CODE>1</CODE>
            //  </ERROR>
            //</VIEWRANGER>

            var messageElement = errorElement.Descendants("MESSAGE");
            var codeElement = errorElement.Descendants("CODE");
            var message = messageElement.Count() > 0 ? messageElement.First().Value : null;
            var code = codeElement.Count() > 0 ? codeElement.First().Value : null;
            return new FailedRequestException(code, message);

            //Error messages and codes
            //Code	Message
            //1	Invalid API key

            //Thrown if:
            //The key parameter was not specified, or
            //Is empty, or
            //Isn't registered in our database
            //2	API key is not enabled

            //Thrown if:
            //The key is registered in our database, but has been disabled for some reason
            //3	API call requested from an incorrect domain

            //Thrown if:
            //The domain which was specified in the registration form (when you applied for the API key) is different from the domain from which the requests were made
            //4	Invalid service

            //Thrown if:
            //The service parameter was not specified, or
            //It is empty, or
            //There is no such service available
            //5	You have reached the usage limit for this service in this period of time

            //Thrown if:
            //Each API key has a limit on how many requests they can make during a fixed period of time (in a day/week/month). If you exceed that number you will receive this error.
            //6	Internal error

            //Thrown if:
            //An internal service error has occurred. This is out of your control. If it persists over a longer period of time, feel free to contact us.
            //8	Invalid date specified

            //Thrown if:
            //The date parameter was specified, but does not follow the required format: YYYY-MM-DD HH:II:SS. For example: 2011-11-27 10:45:59.
            //9	Invalid limit specified

            //Thrown if:
            //The limit parameter was specified, but does not fall into the specified range. See parameter list for details.
            //10	The PIN does not match with the username

            //Thrown if:
            //The BuddyBeacon username exists in our database, but the corresponding password is wrong.
            //11	Username does not exist

            //Thrown if:
            //The BuddyBeacon username does not exist in our database.
            //- See more at: http://www.viewranger.com/developers/documentation/#sthash.wi7BbDza.meHteX6u.dpuf     
        }

        public virtual string CreateUrl(params RequestParameter[] parameters)
        {
            return string.Concat(this.BaseAddress, "?", string.Join("&", parameters.Select(x => string.Concat(x.Key, "=", x.Value)).ToArray()));
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
                if (!string.IsNullOrWhiteSpace(matches.Single().Value))
                {
                    return innerFunction(matches.Single());
                }
                else
                {
                    return null;
                }
            }
            else // > 1
            {
                s_log.ErrorFormat("The LOCATION element had {0} {1} elements", matches.Count(), key);
                throw new UnexpectedResponseException("The LOCATION element had {0} {1} elements", matches.Count(), key);
            }
        }
    }
}