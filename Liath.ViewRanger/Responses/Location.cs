using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liath.ViewRanger.Responses
{
    /// <summary>
    /// Represents a location returned by ViewRanger
    /// </summary>
    public partial class Location
    {
        /// <summary>
        /// The user's latitude
        /// </summary>
        public decimal? Latitude { get; set; }

        /// <summary>
        /// The user's longitude
        /// </summary>
        public decimal? Longitude { get; set; }

        /// <summary>
        /// The date/time the BuddyBeacon was sent
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        /// The altitude the user was at when recording the BuddyBeacon (Metres above sea level)
        /// </summary>
        public decimal? Altitude { get; set; }

        /// <summary>
        /// The speed the user was travelling at when the BuddyBeacon was sent (km per hour)
        /// </summary>
        public decimal? Speed { get; set; }

        /// <summary>
        /// The direction the user was travelling in when the BuddyBeacon was sent (degrees from grid north, for the WGS84 datum)
        /// </summary>
        public decimal? Heading { get; set; }
    }
}
