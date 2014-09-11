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
        private string _baseAddress;

        public ViewRangerClient(string applicationKey, string baseAddress = @"http://api.viewranger.com/public/v1/")
        {
            if (applicationKey == null) throw new ArgumentNullException("applicationKey");
            if (baseAddress == null) throw new ArgumentNullException("baseAddress");

            _baseAddress = baseAddress;
            _applicationKey = applicationKey;
        }

        public IGetLastPositionRequest GetLastPosition()
        {
            return new GetLastPositionRequest(_applicationKey, _baseAddress);
        }


        public IGetTrackRequest GetTrack()
        {
            return new GetTrackRequest(_applicationKey, _baseAddress);
        }
    }
}
