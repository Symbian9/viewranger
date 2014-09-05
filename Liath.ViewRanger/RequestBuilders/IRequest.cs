using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Liath.ViewRanger.RequestBuilders
{
    public interface IRequest<TRequest>
    {
        TRequest Request();
    }
}
