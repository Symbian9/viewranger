using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liath.ViewRanger.Exceptions
{
    /// <summary>
    /// The exception thrown when there was an internal error in ViewRanger's API
    /// </summary>
    public class InternalViewRangerException : FailedRequestException
    {
        public InternalViewRangerException(string code, string message)
            : base(code, message)
        {

        }
        /// <summary>
        /// The error codes for which this application should be thrown
        /// </summary>
        public static string[] ApplicableErrorCodes = new string[] { "6" };
    }
}
