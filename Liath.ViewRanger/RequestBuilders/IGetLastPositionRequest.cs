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
        /// <summary>
        /// Specifies the credentials for the user being queried
        /// </summary>
        IGetLastPositionRequest ForUser(string username, string pin);
    }
}
