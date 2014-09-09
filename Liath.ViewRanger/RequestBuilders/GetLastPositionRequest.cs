using Liath.ViewRanger.Exceptions;
using Liath.ViewRanger.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Liath.ViewRanger.RequestBuilders
{
    public class GetLastPositionRequest : RequestBase, IGetLastPositionRequest
    {
        public const string Service = "getLastBBPosition";
        public string Username { get; set; }
        public string Pin { get; set; }

        public GetLastPositionRequest(string key)
            : base(key)
        {
        }

        public IGetLastPositionRequest ForUser(string username, string pin)
        {
            if (username == null) throw new ArgumentNullException("username");
            if (pin == null) throw new ArgumentNullException("pin");

            this.Username = username;
            this.Pin = pin;
            return this;
        }

        public Location Request()
        {
            var xml = this.MakeRequest(Service, this.Username, this.Pin);
            var locationElements = xml.Descendants("LOCATION");
            if(locationElements.Count() == 1)
            {
                return this.CreateLocationFromXml(locationElements.Single());
            }
            else
            {
                throw new UnexpectedResponseException("{0} LOCATION elements were found in the response", locationElements.Count());
            }
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