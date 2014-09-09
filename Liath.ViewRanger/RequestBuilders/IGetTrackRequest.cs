﻿using Liath.ViewRanger.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Liath.ViewRanger.RequestBuilders
{
    public interface IGetTrackRequest : IRequest<Track>
    {
        /// <summary>
        /// Specifies the user to make the request for
        /// </summary>
        /// <param name="username">The user's username</param>
        /// <param name="pin">The user's PIN</param>
        IGetTrackRequest ForUser(string username, string pin);

        /// <summary>
        /// The earliest time to search from
        /// </summary>
        IGetTrackRequest From(DateTime from);

        /// <summary>
        /// The latest time to search to
        /// </summary>
        IGetTrackRequest To(DateTime to);

        /// <summary>
        /// The maximum number of results to return
        /// </summary>
        IGetTrackRequest Limit(int limit);
    }
}
