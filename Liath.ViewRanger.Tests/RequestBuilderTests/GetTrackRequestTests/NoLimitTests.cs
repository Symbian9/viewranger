using Liath.ViewRanger.RequestBuilders;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liath.ViewRanger.Tests.RequestBuilderTests.GetTrackRequestTests
{
    [TestFixture]
    public class NoLimitTests
    {
        [Test]
        public void Ensure_returns_self()
        {
            var request = new GetTrackRequest(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            var returnedRequest = request.NoLimit();

            Assert.AreSame(request, returnedRequest);
        }

        [Test]
        public void Ensure_limit_is_null()
        {
            var request = ((GetTrackRequest)new GetTrackRequest(Guid.NewGuid().ToString(), Guid.NewGuid().ToString()).NoLimit());
            Assert.IsNull(request.LimitValue);
        }
    }
}
