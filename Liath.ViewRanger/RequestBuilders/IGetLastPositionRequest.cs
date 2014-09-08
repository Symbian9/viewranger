using Liath.ViewRanger.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liath.ViewRanger.RequestBuilders
{
    public interface IGetLastPositionRequest : IRequest<Location>
    {
        IGetLastPositionRequest ForUser(string username, string pin);
    }
}
