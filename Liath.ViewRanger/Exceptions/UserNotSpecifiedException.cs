using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liath.ViewRanger.Exceptions
{
    /// <summary>
    /// The Exception thrown when a request is made but the BuddyBeacon credentials were not set
    /// </summary>
    public class UserNotSpecifiedException : ViewRangerException
    {
        public UserNotSpecifiedException()
            :base("The user was not set for the request the ForUser() method must be called")
        {

        }
    }
}
