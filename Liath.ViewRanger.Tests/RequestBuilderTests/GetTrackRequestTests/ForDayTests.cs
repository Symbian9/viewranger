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
    public class ForDayTests
    {
        [Test]
        public void Ensure_returns_self()
        {
            var request = new GetTrackRequest(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            var returnedRequest = request.ForDay(DateTime.Now);

            Assert.AreSame(request, returnedRequest);
        }

        [Test]
        public void Ensure_from_is_correct()
        {
            var date = new DateTime(2014, 9, 16, 15, 35, 12);
            var request = ((GetTrackRequest)new GetTrackRequest(Guid.NewGuid().ToString(), Guid.NewGuid().ToString()).ForDay(date));
            Assert.AreEqual(date.Date, request.FromDate);
        }

        [Test]
        public void Ensure_to_is_correct()
        {
            var date = new DateTime(2014, 9, 16, 15, 35, 12);
            var request = ((GetTrackRequest)new GetTrackRequest(Guid.NewGuid().ToString(), Guid.NewGuid().ToString()).ForDay(date));
            Assert.AreEqual(date.Date.AddDays(1), request.ToDate);
        }

        [Test]
        public void Ensure_midnight_sets_to_upcoming_day()
        {
            // If someone gives us a date we need to ensure that the request limits to the date in question not the following one
            var date = new DateTime(2014, 9, 16);
            var request = ((GetTrackRequest)new GetTrackRequest(Guid.NewGuid().ToString(), Guid.NewGuid().ToString()).ForDay(date));
            Assert.AreEqual(date.Date, request.FromDate);
            Assert.AreEqual(date.Date.AddDays(1), request.ToDate);
        }
    }
}
