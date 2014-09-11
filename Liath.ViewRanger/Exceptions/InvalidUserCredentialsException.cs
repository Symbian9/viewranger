using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liath.ViewRanger.Exceptions
{
    /// <summary>
    /// The exception thrown when the user's credentials were rejected
    /// </summary>
    public class InvalidUserCredentialsException : FailedRequestException
    {
        public InvalidUserCredentialsException(string code, string message)
            : base(code, message)
        {

        }

        //10 The PIN does not match with the username
        //11 Username does not exist

        /// <summary>
        /// The error codes for which this application should be thrown
        /// </summary>
        public static string[] ApplicableErrorCodes = new string[] { "10", "11" };
    }
}
