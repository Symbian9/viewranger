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
    public class ForTodayTests
    {
        [Test]
        public void Ensure_returns_self()
        {
            var request = new GetTrackRequest(Guid.NewGuid().ToString());
            var returnedRequest = request.ForToday();

            Assert.AreSame(request, returnedRequest);
        }

        [Test]
        public void Ensure_from_is_correct()
        {
            var now = DateTime.Now; // there is a VERY small chance that if this is run at exactly midnight this will fail... VERY VERY VERY!
            var request = ((GetTrackRequest)new GetTrackRequest(Guid.NewGuid().ToString()).ForToday());
            Assert.AreEqual(now.Date, request.FromDate);
        }

        [Test]
        public void Ensure_to_is_correct()
        {
            var now = DateTime.Now; // there is a VERY small chance that if this is run at exactly midnight this will fail... VERY VERY VERY!
            var request = ((GetTrackRequest)new GetTrackRequest(Guid.NewGuid().ToString()).ForToday());            
            Assert.AreEqual(now.Date.AddDays(1), request.ToDate);
        }
    }
}
