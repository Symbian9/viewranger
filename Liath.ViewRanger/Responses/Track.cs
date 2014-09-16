using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liath.ViewRanger.Responses
{
    /// <summary>
    /// The track is made up of locations recorded via BuddyBeacon
    /// </summary>
    public partial class Track
    {
        /// <summary>
        /// The BuddyBeacon locations recorded
        /// </summary>
        public IEnumerable<Location> Locations { get; set; }

        /// <summary>
        /// The time of the earliest waypoint recorded
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// The time of the last waypoint recorded
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// The time between the first and last recorded waypoints
        /// </summary>
        public TimeSpan? Duration { get; set; }
    }
}
