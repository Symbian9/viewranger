using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liath.ViewRanger.Exceptions
{
    public class FailedRequestException : ViewRangerException
    {
        /// <summary>
        /// The Error Code returned from ViewRanger
        /// </summary>
        public string ViewRangerCode { get; set; }

        /// <summary>
        /// The ErrorMessage ViewRanger returned
        /// </summary>
        public string ViewRangerMessage { get; set; }

        public FailedRequestException(string code, string message)
            :base("ViewRanger was unable to complete the request")
        {
            this.ViewRangerMessage = message;
            this.ViewRangerCode = code;
        }
    }
}
