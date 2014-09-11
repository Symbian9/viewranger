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
    /// The request object which gets the user's last position
    /// </summary>
    public class GetLastPositionRequest : RequestBase, IGetLastPositionRequest
    {
        private static ILog s_log = LogManager.GetLogger(typeof(GetLastPositionRequest));

        /// <summary>
        /// The name of the function which we call on the ViewRanger service
        /// </summary>
        /// 
        public override string Service
        {
            get
            {
                return "getLastBBPosition";
            }
        }

        /// <summary>
        /// Creates a new request based using the ApplicationKey
        /// </summary>
        /// <param name="key">The ApplicationKey used to call ViewRanger</param>
        public GetLastPositionRequest(string key, string baseAddress)
            : base(key, baseAddress)
        {
        }

        /// <summary>
        /// Specifies the uer being queried
        /// </summary>
        public IGetLastPositionRequest ForUser(string username, string pin)
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

        /// <summary>
        /// Makes the request from ViewRanger
        /// </summary>
        /// <returns>The user's last location</returns>
        public Location Request()
        {
            return this.HandleExceptions(s_log, () =>
               {
                   var xml = this.MakeRequest();
                   var locationElements = xml.Descendants("LOCATION");
                   if (locationElements.Count() == 1)
                   {
                       return this.CreateLocationFromXml(locationElements.Single());
                   }
                   else
                   {
                       var message = string.Format("{0} LOCATION elements were found in the response", locationElements.Count());
                       s_log.Error(message);
                       throw new UnexpectedResponseException(message);
                   }
               });

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
        }        
    }
}