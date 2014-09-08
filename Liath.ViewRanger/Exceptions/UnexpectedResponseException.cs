using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liath.ViewRanger.Exceptions
{
    public class UnexpectedResponseException : ViewRangerException
    {
        public UnexpectedResponseException(string message)
            :base(message)
        {

        }

        public UnexpectedResponseException(string message, params object[] args)
            : base(string.Format(message, args))
        {

        }
    }
}
