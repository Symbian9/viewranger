using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liath.ViewRanger.Exceptions
{
    /// <summary>
    /// All Exceptions thrown by this library should inherit from this
    /// </summary>
    public class ViewRangerException : Exception
    {
        public ViewRangerException(string message)
            :base(message)
        {

        }

        public ViewRangerException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
