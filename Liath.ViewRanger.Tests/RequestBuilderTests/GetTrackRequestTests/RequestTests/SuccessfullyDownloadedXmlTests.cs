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

namespace Liath.ViewRanger.Tests.RequestBuilderTests.GetTrackRequestTests.RequestTests
{
    [TestFixture]
    public class SuccessfullyDownloadedXmlTests
    {
        [Test]
        public void Ensure_track_is_populated()
        {
            var request = new Mock<GetTrackRequest>(Guid.NewGuid().ToString());
            request.Setup(x => x.DownloadXml(It.IsAny<string>())).Returns(SampleResponse.Successful);

            var track = request.Object.Request();

            this.AssertSuccessfulTrackIsCorrect(track);
        }

        private void AssertSuccessfulTrackIsCorrect(Track track)
        {
            var xml = SampleResponse.Successful.Descendants("LOCATION").Select(n => new
            {
                LATITUDE = decimal.Parse(n.Descendants("LATITUDE").Single().Value),
                LONGITUDE = decimal.Parse(n.Descendants("LONGITUDE").Single().Value),
                DATE = DateTime.Parse(n.Descendants("DATE").Single().Value),
                ALTITUDE =decimal.Parse( n.Descendants("ALTITUDE").Single().Value),
                SPEED = decimal.Parse(n.Descendants("SPEED").Single().Value),
                HEADING = decimal.Parse(n.Descendants("HEADING").Single().Value)
            });

            Assert.IsNotNull(track);
            Assert.IsNotNull(track.Locations);
            Assert.AreEqual(xml.Count(), track.Locations.Count());

            for(int i = 0; i < 3; i++)
            {
                Assert.AreEqual(xml.ElementAt(i).LATITUDE, track.Locations.ElementAt(i).Latitude);
                Assert.AreEqual(xml.ElementAt(i).LONGITUDE, track.Locations.ElementAt(i).Longitude);
                Assert.AreEqual(xml.ElementAt(i).DATE, track.Locations.ElementAt(i).Date);
                Assert.AreEqual(xml.ElementAt(i).ALTITUDE, track.Locations.ElementAt(i).Altitude);
                Assert.AreEqual(xml.ElementAt(i).SPEED, track.Locations.ElementAt(i).Speed);
                Assert.AreEqual(xml.ElementAt(i).HEADING, track.Locations.ElementAt(i).Heading);
            }
        }
    }
}
