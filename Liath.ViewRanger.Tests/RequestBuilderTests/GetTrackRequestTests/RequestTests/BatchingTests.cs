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
    public class BatchingTests
    {
        [Test]
        public void Ensure_limits_are_honoured_when_NoLimit_not_called()
        {
            var limit = 10;
            var request = new Mock<GetTrackRequest>(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            request.CallBase = true;
            request.Setup(r => r.MakeRequest(It.IsAny<RequestParameter[]>()))
                .Returns(SampleResponse.Successful)
                .Callback<RequestParameter[]>(parameters =>
            {
                Assert.AreEqual(limit.ToString(), parameters.Single(p => p.Key == GetTrackRequest.LimitKey).Value);
            });
            request.Object.Limit(limit).Request();
        }

        [Test]
        public void Ensure_single_batch_only_requests_once()
        {
            var limit = 10;
            var xml = this.CreateXmlForLocations(this.CreateLocations(limit, new DateTime(2010, 1, 1))); // even if the xml response is full
            var request = new Mock<GetTrackRequest>(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            request.CallBase = true;
            request.Setup(r => r.MakeRequest(It.IsAny<RequestParameter[]>())).Returns(xml).Verifiable();
            request.Object.Limit(limit).Request();

            request.Verify(x => x.MakeRequest(It.IsAny<RequestParameter[]>()), Times.Once());
        }

        [Test]
        public void Ensure_requests_terminate_when_fewer_than_max_returned()
        {
            int requestCount = 0;
            int timesToCall = 5;
            var request = new Mock<GetTrackRequest>(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            request.CallBase = true;
            request.Setup(r => r.MakeRequest(It.IsAny<RequestParameter[]>()))
                .Callback(() => { requestCount++;})
                .Returns(() =>
                    {
                        var countToReturn = requestCount == timesToCall ? 123 : GetTrackRequest.MaxMatchSize; // the first two times we'll return the maximum, then we'll return less
                        return this.CreateXmlForLocations(this.CreateLocations(countToReturn, new DateTime(2010, 1, 1)));
                    })
                .Verifiable();
            request.Object.NoLimit().Request();

            request.Verify(x => x.MakeRequest(It.IsAny<RequestParameter[]>()), Times.Exactly(timesToCall));
        }

        [Test]
        public void Ensure_request_terminates_when_zero_results_returned()
        {
            int requestCount = 0;
            int timesToCall = 4;
            var request = new Mock<GetTrackRequest>(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            request.CallBase = true;
            request.Setup(r => r.MakeRequest(It.IsAny<RequestParameter[]>()))
                .Callback(() => { requestCount++; })
                .Returns(() =>
                {
                    var countToReturn = requestCount == timesToCall ? 0 : GetTrackRequest.MaxMatchSize; // the first two times we'll return the maximum, then we'll return none
                    return this.CreateXmlForLocations(this.CreateLocations(countToReturn, new DateTime(2010, 1, 1)));
                })
                .Verifiable();
            request.Object.NoLimit().Request();

            request.Verify(x => x.MakeRequest(It.IsAny<RequestParameter[]>()), Times.Exactly(timesToCall));
        }

        [Test]
        public void Ensure_ToFilter_recalculated_correctly()
        {
            int requestCount = 0;
            var minDateTime = new DateTime(2014,1,2,18,4,14);
            var request = new Mock<GetTrackRequest>(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            request.CallBase = true;
            request.Setup(r => r.MakeRequest(It.IsAny<RequestParameter[]>()))
                .Returns(() => {
                    var resultCountToReturn = requestCount == 0 ? 500 : 0;
                    var resultsToReturn = this.CreateLocations(resultCountToReturn, minDateTime);
                    var xml = this.CreateXmlForLocations(resultsToReturn);
                    return xml;
                })
                .Callback<RequestParameter[]>(parameters =>
                { 
                    requestCount++;
                    if(requestCount == 2) // only bother checking on our second request
                    {
                        var toParam = parameters.Single(p => p.Key == GetTrackRequest.ToKey);
                        Assert.AreEqual(minDateTime.AddSeconds(-1), DateTime.Parse(toParam.Value));
                    }
                    else if(requestCount > 2) // it's a complicated test - sanity check
                    {
                        throw new InvalidProgramException("This test should not reach requestCount > 2");
                    }
                });
            request.Object.NoLimit().Request();

            Assert.AreEqual(2, requestCount); // this is just a check to ensure that the AssertMethods were called properly
        }

        private XDocument CreateXmlForLocations(IEnumerable<Location> locations)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sb.AppendLine("<VIEWRANGER>");
            foreach (var location in locations)
            {
                sb.AppendLine("<LOCATION>");
                sb.AppendLine(string.Concat("<LATITUDE>", location.Latitude, "</LATITUDE>"));
                sb.AppendLine(string.Concat("<LONGITUDE>", location.Longitude, "</LONGITUDE>"));
                sb.AppendLine(string.Concat("<DATE>", location.Date, "</DATE>"));
                sb.AppendLine(string.Concat("<ALTITUDE>", location.Altitude, "</ALTITUDE>"));
                sb.AppendLine(string.Concat("<SPEED>", location.Speed, "</SPEED>"));
                sb.AppendLine(string.Concat("<HEADING>", location.Heading, "</HEADING>"));
                sb.AppendLine("</LOCATION>");
            }

            sb.AppendLine("</VIEWRANGER>");

            return XDocument.Parse(sb.ToString());
        }

        private IEnumerable<Location> CreateLocations(int count, DateTime minDateTime)
        {
            List<Location> locations = new List<Location>();
            for (int i = 0; i < count; i++)
            {
                locations.Add(new Location
                    {
                        Date = minDateTime.AddMinutes(i)
                    });
            }

            return locations;
        }
    }
}
