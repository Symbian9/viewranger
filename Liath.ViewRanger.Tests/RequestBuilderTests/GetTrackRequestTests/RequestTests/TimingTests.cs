using Liath.ViewRanger.RequestBuilders;
using Liath.ViewRanger.Responses;
using Liath.ViewRanger.Tests.RequestBuilderTests.GetTrackRequestTests.RequestTests.SampleResponses;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Liath.ViewRanger.Tests.RequestBuilderTests.GetTrackRequestTests.RequestTests
{
    [TestFixture]
    public class TimingTests
    {
        [Test]
        public void Ensure_StartTime_is_correct()
        {
            var track = this.GetTrackFromResponse(SampleResponse.Successful);
            Assert.AreEqual(new DateTime(2011, 09, 21, 15, 03, 05), track.StartTime);
        }

        [Test]
        public void Ensure_EndTime_is_correct()
        {
            var track = this.GetTrackFromResponse(SampleResponse.Successful);
            Assert.AreEqual(new DateTime(2011, 09, 21, 15, 11, 05), track.EndTime);
        }

        [Test]
        public void Ensure_StartTime_is_null_when_no_locations()
        {
            var track = this.GetTrackFromResponse(SampleResponse.NoLocations);
            Assert.IsNull(track.EndTime);
        }

        [Test]
        public void Ensure_EndTime_is_null_when_no_locations()
        {
            var track = this.GetTrackFromResponse(SampleResponse.NoLocations);
            Assert.IsNull(track.EndTime);
        }

        [Test]
        public void Ensure_Duration_is_correct()
        {
            var track = this.GetTrackFromResponse(SampleResponse.Successful);
            Assert.AreEqual(new TimeSpan(0, 8, 0), track.Duration);
        }

        [Test]
        public void Ensure_Duration_is_null_when_no_locations()
        {
            var track = this.GetTrackFromResponse(SampleResponse.NoLocations);
            Assert.IsNull(track.Duration);
        }

        public Track GetTrackFromResponse(XDocument xml)
        {
            var request = new Mock<GetTrackRequest>(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            request.CallBase = true;
            request.Setup(x => x.MakeRequest(It.IsAny<RequestParameter[]>())).Returns(xml);
            return request.Object.ForUser("user", "1234").ForToday().Request();
        }
    }
}
