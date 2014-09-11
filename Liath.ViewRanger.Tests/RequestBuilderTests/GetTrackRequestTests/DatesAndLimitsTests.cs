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
    public class DatesAndLimitsTests
    {
        [Test]
        public void Ensure_FromDate_is_set()
        {
            var now = DateTime.Now;
            var req = new GetTrackRequest(Guid.NewGuid().ToString(), Guid.NewGuid().ToString()).From(now);
            Assert.AreEqual(now, ((GetTrackRequest)req).FromDate);
        }

        [Test]
        public void Ensure_ToDate_is_set()
        {
            var now = DateTime.Now;
            var req = new GetTrackRequest(Guid.NewGuid().ToString(), Guid.NewGuid().ToString()).To(now);
            Assert.AreEqual(now, ((GetTrackRequest)req).ToDate);
        }

        [Test]
        public void Ensure_LimitValue_is_set()
        {
            var limit = 10;
            var req = new GetTrackRequest(Guid.NewGuid().ToString(), Guid.NewGuid().ToString()).Limit(limit);
            Assert.AreEqual(limit, ((GetTrackRequest)req).LimitValue);
        }
    }
}
