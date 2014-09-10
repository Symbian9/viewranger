using Liath.ViewRanger.RequestBuilders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liath.ViewRanger
{
    public class ViewRangerClient : IViewRangerClient
    {
        private string _applicationKey;

        public ViewRangerClient(string applicationKey)
        {
            if (applicationKey == null) throw new ArgumentNullException("applicationKey");

            _applicationKey = applicationKey;
        }

        public IGetLastPositionRequest GetLastPosition()
        {
            return new GetLastPositionRequest(_applicationKey);
        }


        public IGetTrackRequest GetTrack()
        {
            return new GetTrackRequest(_applicationKey);
        }
    }
}
