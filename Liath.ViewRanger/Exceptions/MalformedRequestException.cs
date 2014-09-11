using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liath.ViewRanger.Exceptions
{
    /// <summary>
    /// The exception thrown when the parameters sent to the API were invalid
    /// </summary>
    public class MalformedRequestException : FailedRequestException
    {
        public MalformedRequestException(string code, string message)
            : base(code, message)
        {

        }

        //4 Invalid service
        //8 Invalid date specified
        //9 Invalid limit specified

        /// <summary>
        /// The error codes for which this application should be thrown
        /// </summary>
        public static string[] ApplicableErrorCodes = new string[] { "4", "8", "9" };
    }
}
