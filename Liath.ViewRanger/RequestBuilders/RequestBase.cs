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
    /// <summary>
    /// The BaseClass for all ViewRanger requests
    /// </summary>
    public abstract class RequestBase
    {
        /// <summary>
        /// Our logger
        /// </summary>
        private static ILog s_log = LogManager.GetLogger(typeof(RequestBase));

        /// <summary>
        /// The format to download responses in
        /// </summary>
        protected const string RequestFormat = "xml";

        /// <summary>
        /// The BaseAddress of the ViewRanger API
        /// </summary>
        public virtual string BaseAddress { get; set; }

        /// <summary>
        /// The service to call
        /// </summary>
        public abstract string Service { get; }

        /// <summary>
        /// The ApplicationKey to use when calling the ViewRanger API
        /// </summary>
        public string ApplicationKey { get; set; }

        /// <summary>
        /// The user's username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The user's PIN
        /// </summary>
        public string Pin { get; set; }

        // Keys
        public const string KeyKey = "key";
        public const string ServiceKey = "service";
        public const string UsernameKey = "username";
        public const string PinKey = "pin";
        public const string FormatKey = "format";

        /// <summary>
        /// Creates a new request
        /// </summary>
        protected RequestBase(string applicationKey, string baseAddress)
        {
            if (applicationKey == null) throw new ArgumentNullException("applicationKey");
            if (baseAddress == null) throw new ArgumentNullException("baseAddress");

            this.BaseAddress = baseAddress;
            this.ApplicationKey = applicationKey;
        }

        /// <summary>
        /// Makes the request to the ViewRanger API, checks for errors and returns the returned xml
        /// </summary>
        /// <returns>The XML returned</returns>
        public virtual XDocument MakeRequest(params RequestParameter[] parameters)
        {
            if(this.Username == null || this.Pin == null)
            {
                s_log.Error("Could not complete request because either the username or PIN were null");
                throw new UserNotSpecifiedException();
            }

            var allParameters = new RequestParameter[]
            {
                new RequestParameter(KeyKey, ApplicationKey),
                new RequestParameter(ServiceKey, this.Service),
                new RequestParameter(UsernameKey, this.Username),
                new RequestParameter(PinKey, this.Pin),
                new RequestParameter(FormatKey, RequestFormat)
            }.Union(parameters).ToArray();

            var url = this.CreateUrl(allParameters);

            // Strip out the username/password/key from the logs
            var safeMessageToLog = this.RemoveSensitiveInformation(url);
            s_log.DebugFormat("Attempting to download data from '{0}'", safeMessageToLog);

            var document = this.DownloadXml(url);
            s_log.Debug("XML response downloaded");

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

        /// <summary>
        /// Usernames and PINs are often included in things like URLs, this method will remove
        /// </summary>
        private string RemoveSensitiveInformation(string message)
        {
            // Strip out in order of complexity - avoids edge case where the PIN is part of the key or username
            string strippedData = this.RemoveSensitiveInformation(message, this.ApplicationKey);
            strippedData = this.RemoveSensitiveInformation(strippedData, this.Username);
            strippedData = this.RemoveSensitiveInformation(strippedData, this.Pin);

            return strippedData;
        }

        /// <summary>
        /// Removes the valueToRemove from the message and replaces it with asterisks
        /// </summary>
        private string RemoveSensitiveInformation(string message, string valueToRemove)
        {
            return message.Replace(valueToRemove, new string('*', valueToRemove.Length));
        }

        /// <summary>
        /// Checks if the XML contains an error element and throws appropriate exceptions
        /// </summary>
        /// <param name="document">The XML document returned from ViewRanger</param>
        private void HandleErrors(XDocument document)
        {
            s_log.Debug("Checking XML response for errors");
            var errorElements = document.Descendants("ERROR");
            if(errorElements.Count() > 0)
            {
                s_log.ErrorFormat("{0} errors detected", errorElements.Count());
                var firstErrorElement = errorElements.First();
                var ex = this.CreateExceptionFromError(firstErrorElement);
                throw ex;
            }
            s_log.Debug("XML response contained no errors");
        }

        /// <summary>
        /// Parses an error element and creates an exception
        /// </summary>
        /// <param name="errorElement">The XML element containing the errors</param>
        /// <returns>The exception to throw</returns>
        private ViewRangerException CreateExceptionFromError(XElement errorElement)
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

            s_log.DebugFormat("Error response was found with Message '{0}' and Code '{1}', creating exception.", message, code);

            if(ApiKeyRejectedException.ApplicableErrorCodes.Contains(code))
            {
                throw new ApiKeyRejectedException(code, message);
            }
            if(MalformedRequestException.ApplicableErrorCodes.Contains(code))
            {
                throw new MalformedRequestException(code, message);
            }
            if(InvalidUserCredentialsException.ApplicableErrorCodes.Contains(code))
            {
                throw new InvalidUserCredentialsException(code, message);
            }
            if(InternalViewRangerException.ApplicableErrorCodes.Contains(code))
            {
                throw new InternalViewRangerException(code, message);
            }

            // The obove checks should catch all error codes however add a catch all in case new ones are introduced
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

        /// <summary>
        /// Creates the URL to call from the BaseAddress and supplied parameters
        /// </summary>
        /// <param name="parameters">The parameters to add to the QueryString</param>
        /// <returns>The Request URL</returns>
        public virtual string CreateUrl(params RequestParameter[] parameters)
        {
            s_log.DebugFormat("Building URL with BaseAddress '{0}' and {1} parameters", this.BaseAddress, parameters.Count());
            return string.Concat(this.BaseAddress, "?", string.Join("&", parameters.Select(x => string.Concat(x.Key, "=", x.Value)).ToArray()));
        }

        /// <summary>
        /// Downloads XML from the given URL
        /// </summary>
        /// <remarks>This method is a public virtual to allow for easy mocking</remarks>
        /// <param name="url">The URL to download from</param>
        /// <returns>The XML downloaded</returns>
        public virtual XDocument DownloadXml(string url)
        {
            s_log.Debug("Downloading the XML document");
            return XDocument.Load(url);
        }

        /// <summary>
        /// Parses a LocationElement and returns a Location object
        /// </summary>
        /// <param name="locationElement">The XML representing the Location</param>
        /// <returns>A populated Location object</returns>
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

        /// <summary>
        /// Checks the element for a child identified by the key, attempts to parse it and returns the result
        /// </summary>
        /// <param name="element">The Location XML Element</param>
        /// <param name="key">The child element name to search for</param>
        /// <returns>The decimal or Null if the element was not present</returns>
        /// <remarks>Will throw an UnexpectedResponseException if the value cannot be parsed or more than one matching child is found</remarks>
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

        /// <summary>
        /// Checks the element for a child identified by the key, attempts to parse it and returns the result
        /// </summary>
        /// <param name="element">The Location XML Element</param>
        /// <param name="key">The child element name to search for</param>
        /// <returns>The DateTime or Null if the element was not present</returns>
        /// <remarks>Will throw an UnexpectedResponseException if the value cannot be parsed or more than one matching child is found</remarks>
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

        /// <summary>
        /// A generic GetXValue method, allows you to pass in manual parsing code
        /// </summary>
        /// <typeparam name="T">The type to parse too</typeparam>
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

        /// <summary>
        /// Runs the requested function wrapped in a Try/Catch which logs and throws appropriate exceptions
        /// </summary>
        /// <param name="logger">The logger errors should be logged to</param>
        /// <param name="func">The action to attempt</param>
        protected T HandleExceptions<T>(ILog logger, Func<T> func)
        {
            try
            {
                return func();
            }
            catch(ViewRangerException ex)
            {
                logger.Error("There was an error accessing ViewRanger", ex);
                throw;
            }
            catch(Exception ex)
            {
                logger.Error("There was an error processing the request", ex);
                throw new ClientException(ex);
            }
        }
    }
}