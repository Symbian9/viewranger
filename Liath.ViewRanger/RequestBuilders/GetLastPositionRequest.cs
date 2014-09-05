using Liath.ViewRanger.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liath.ViewRanger.RequestBuilders
{
    public class GetLastPositionRequest : RequestBase, IGetLastPositionRequest
    {
        public IGetLastPositionRequest ForUser(string username)
        {
            throw new NotImplementedException();
        }

        public Position Request()
        {
            throw new NotImplementedException();
        }
    }
}
