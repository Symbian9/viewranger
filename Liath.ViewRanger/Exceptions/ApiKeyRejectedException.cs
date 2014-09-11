using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liath.ViewRanger.Exceptions
{
    /// <summary>
    /// The exception thrown when the API key was rejected by ViewRanger
    /// </summary>
    public class ApiKeyRejectedException : FailedRequestException
    {
        public ApiKeyRejectedException(string code, string message)
            :base(code, message)
        {

        }

        //1 Invalid API key
        //2 API key is not enabled
        //3 API call requested from an incorrect domain
        //5 You have reached the usage limit for this service in this period of time

        /// <summary>
        /// The error codes for which this application should be thrown
        /// </summary>
        public static string[] ApplicableErrorCodes = new string[] { "1", "2", "3", "5" };
    }
}
