using Liath.ViewRanger.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liath.ViewRanger.RequestBuilders
{
    public interface IGetLastPositionRequest : IRequest<Position>
    {
        IGetLastPositionRequest ForUser(string username);        
    }
}
