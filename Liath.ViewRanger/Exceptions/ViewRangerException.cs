using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liath.ViewRanger.Exceptions
{
    public class ViewRangerException : Exception
    {
        public ViewRangerException(string message)
            :base(message)
        {

        }
    }
}
