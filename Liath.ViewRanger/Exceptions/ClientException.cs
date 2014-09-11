using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liath.ViewRanger.Exceptions
{
    /// <summary>
    /// The exception thrown if there was a problem with this library
    /// </summary>
    public class ClientException : Exception
    {
        public ClientException(Exception innerException)
            : base("There was an error in the ViewRanger API Client Library, see the InnerException for more details", innerException)
        {
        }
    }
}
