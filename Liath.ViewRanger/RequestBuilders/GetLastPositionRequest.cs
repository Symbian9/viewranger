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