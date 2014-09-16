using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liath.ViewRanger
{
    /// <summary>
    /// The interface which must be implemented by any user which can access the BuddyBeacon API
    /// </summary>
    public interface IUser
    {
        /// <summary>
        /// The user's username (usually their email address)
        /// </summary>
        string BuddyBeaconUsername { get; }

        /// <summary>
        /// The PIN the user has set up on their BuddyBeacon account
        /// </summary>
        string BuddyBeaconPin { get; }
    }
}
