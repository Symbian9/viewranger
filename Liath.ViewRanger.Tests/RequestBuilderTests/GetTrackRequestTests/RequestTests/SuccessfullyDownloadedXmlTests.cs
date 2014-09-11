using Liath.ViewRanger.Exceptions;
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
            var request = new Mock<GetTrackRequest>(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            request.CallBase = true;
            request.Setup(x => x.DownloadXml(It.IsAny<string>())).Returns(SampleResponse.Successful);

            var track = request.Object
                .ForUser(Guid.NewGuid().ToString(), Guid.NewGuid().ToString())
                .Request();

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
        
        public void Throws_when_no_locations()
        {
            var xml = SampleResponse.NoLocations;
            var request = new Mock<GetLastPositionRequest>(Guid.NewGuid().ToString());
            request.CallBase = true;
            request.Setup(x => x.MakeRequest()).Returns(xml);
            Assert.Throws<UnexpectedResponseException>(() => request.Object.Request());
        }

        #region Nulls

        [Test]
        public void Check_Latitude_is_null_when_ommitted()
        {
            var track = this.GetTrackFromEmptyResponse();
            Assert.IsNull(track.Locations.First().Latitude);
        }

        [Test]
        public void Check_Longitude_is_null_when_ommitted()
        {
            var track = this.GetTrackFromEmptyResponse();
            Assert.IsNull(track.Locations.First().Longitude);
        }

        [Test]
        public void Check_Speed_is_null_when_ommitted()
        {
            var track = this.GetTrackFromEmptyResponse();
            Assert.IsNull(track.Locations.First().Speed);
        }

        [Test]
        public void Check_Altitude_is_null_when_ommitted()
        {
            var track = this.GetTrackFromEmptyResponse();
            Assert.IsNull(track.Locations.First().Altitude);
        }

        [Test]
        public void Check_Heading_is_null_when_ommitted()
        {
            var track = this.GetTrackFromEmptyResponse();
            Assert.IsNull(track.Locations.First().Heading);
        }

        [Test]
        public void Check_Date_is_null_when_ommitted()
        {
            var track = this.GetTrackFromEmptyResponse();
            Assert.IsNull(track.Locations.First().Date);
        }

        #endregion

        #region Blank values

        [Test]
        public void Check_Latitude_is_null_when_blank()
        {
            var track = this.GetLocationFromBlankResponse("LATITUDE");
            Assert.IsNull(track.Locations.First().Latitude);
        }

        [Test]
        public void Check_Longitude_is_null_when_blank()
        {
            var track = this.GetLocationFromBlankResponse("LONGITUDE");
            Assert.IsNull(track.Locations.First().Longitude);
        }

        [Test]
        public void Check_Speed_is_null_when_blank()
        {
            var track = this.GetLocationFromBlankResponse("SPEED");
            Assert.IsNull(track.Locations.First().Speed);
        }

        [Test]
        public void Check_Altitude_is_null_when_blank()
        {
            var track = this.GetLocationFromBlankResponse("ALTITUDE");
            Assert.IsNull(track.Locations.First().Altitude);
        }

        [Test]
        public void Check_Heading_is_null_when_blank()
        {
            var track = this.GetLocationFromBlankResponse("HEADING");
            Assert.IsNull(track.Locations.First().Heading);
        }

        [Test]
        public void Check_Date_is_null_when_blank()
        {
            var track = this.GetLocationFromBlankResponse("DATE");
            Assert.IsNull(track.Locations.First().Date);
        }

        #endregion

        [TestCase("LATITUDE")]
        [TestCase("LONGITUDE")]
        [TestCase("DATE")]
        [TestCase("ALTITUDE")]
        [TestCase("SPEED")]
        [TestCase("HEADING")]
        public void Ensure_throws_when_value_cannot_be_parsed(string node)
        {
            Assert.Throws<UnexpectedResponseException>(() =>
            {
                var location = this.GetLocationFromInvalidResponse(node);
            });
        }

        private Track GetLocationFromInvalidResponse(string nodeName)
        {
            var xml = SampleResponse.Successful;
            xml.Descendants(nodeName).First().Value = Guid.NewGuid().ToString();
            var request = new Mock<GetTrackRequest>(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            request.CallBase = true;
            request.Setup(x => x.MakeRequest(It.IsAny<RequestParameter[]>())).Returns(xml);
            return request.Object
                .ForUser(Guid.NewGuid().ToString(), Guid.NewGuid().ToString())
                .Request();
        }

        private Track GetTrackFromEmptyResponse()
        {
            var request = new Mock<GetTrackRequest>(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            request.CallBase = true;
            request.Setup(x => x.MakeRequest(It.IsAny<RequestParameter[]>())).Returns(SampleResponse.Empty);
            return request.Object
                .ForUser(Guid.NewGuid().ToString(), Guid.NewGuid().ToString())
                .Request();
        }

        private Track GetLocationFromBlankResponse(string nodeName)
        {
            var xml = SampleResponse.Successful;
            xml.Descendants(nodeName).First().Value = string.Empty;
            var request = new Mock<GetTrackRequest>(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());            
            request.CallBase = true;
            request.Setup(x => x.MakeRequest(It.IsAny<RequestParameter[]>())).Returns(xml);
            return request.Object
                .ForUser(Guid.NewGuid().ToString(), Guid.NewGuid().ToString())
                .Request();
        }
    }
}
