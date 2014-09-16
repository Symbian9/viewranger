using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liath.ViewRanger
{
    /// <summary>
    /// Contains Extension methods for IUser
    /// </summary>
    public static class IUserExtensions
    {
        /// <summary>
        /// Returns a bool indicating whether the specified user has their BuddyBeacon username and password populated
        /// </summary>
        public static bool HasCredentials(this IUser user)
        {
            return !string.IsNullOrWhiteSpace(user.BuddyBeaconUsername)
                && !string.IsNullOrWhiteSpace(user.BuddyBeaconPin);
        }
    }
}
