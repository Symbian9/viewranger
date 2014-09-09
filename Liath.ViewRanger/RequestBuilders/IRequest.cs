using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Liath.ViewRanger.RequestBuilders
{
    public interface IRequest<TRequest>
    {
        /// <summary>
        /// Makes the ViewRanger request
        /// </summary>
        TRequest Request();
    }
}
