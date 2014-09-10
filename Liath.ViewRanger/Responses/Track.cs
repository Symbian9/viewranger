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
    }
}
