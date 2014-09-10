using Liath.ViewRanger.RequestBuilders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Liath.ViewRanger
{
    /// <summary>
    /// An interface defining the ViewRangerClient to allow simple mocking for consumers
    /// </summary>
    public interface IViewRangerClient
    {
        /// <summary>
        /// Creates a new request to get the user's last known position
        /// </summary>
        IGetLastPositionRequest GetLastPosition();

        /// <summary>
        /// Creates a new request to download a track
        /// </summary>
        IGetTrackRequest GetTrack();
    }
}
