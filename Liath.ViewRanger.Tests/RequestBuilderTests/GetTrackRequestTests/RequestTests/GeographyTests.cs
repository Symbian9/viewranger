using Liath.ViewRanger.RequestBuilders;
using Liath.ViewRanger.Responses;
using Liath.ViewRanger.Tests.RequestBuilderTests.GetTrackRequestTests.RequestTests.SampleResponses;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Liath.ViewRanger.Tests.RequestBuilderTests.GetTrackRequestTests.RequestTests
{
    [TestFixture]
    public class GeographyTests
    {
        [Test]
        public void Ensure_TotalDistance_is_correct_with_two_points()
        {
            var coords = new GeoCoordinate[]
            { 
                new GeoCoordinate(20, 80),
                new GeoCoordinate(10, 90)
            };

            var expectedDistance = (decimal)coords.ElementAt(0).GetDistanceTo(coords.ElementAt(1));
            var xml = new XDocument(new XElement("VIEWRANGER",
                coords.Select(c => new XElement("LOCATION",
                    new XElement("LATITUDE", c.Latitude),
                    new XElement("LONGITUDE", c.Longitude)))));
            var track = this.GetTrackFromResponse(xml);

            Assert.AreEqual(expectedDistance, track.TotalDistance);
        }

        [Test]
        public void Ensure_TotalDistance_is_correct_with_three_points()
        {
            var coords = new GeoCoordinate[]
            { 
                new GeoCoordinate(20, 80),
                new GeoCoordinate(10, 90),
                new GeoCoordinate(5, 20)
            };

            var edge1 = coords.ElementAt(0).GetDistanceTo(coords.ElementAt(1));
            var edge2 = coords.ElementAt(1).GetDistanceTo(coords.ElementAt(2));
            var expectedDistance = (decimal)(edge1 + edge2);

            var xml = new XDocument(new XElement("VIEWRANGER",
                coords.Select(c => new XElement("LOCATION",
                    new XElement("LATITUDE", c.Latitude),
                    new XElement("LONGITUDE", c.Longitude)))));
            var track = this.GetTrackFromResponse(xml);

            Assert.AreEqual(expectedDistance, track.TotalDistance);
        }

        [Test]
        public void Ensure_TotalDistance_is_null_if_no_locations()
        {
            var track = this.GetTrackFromResponse(SampleResponse.Empty);
            Assert.IsNull(track.TotalDistance);
        }

        [Test]
        public void Ensure_TotalHeightGain_is_correct()
        {
            var heights = new decimal[]{0, 10, 5, 15, 0};
            var expectedIncrease = 20;
            var xml = new XDocument(new XElement("VIEWRANGER",
                heights.Select(h => new XElement("LOCATION", new XElement("ALTITUDE", h)))));
            var track = this.GetTrackFromResponse(xml);

            Assert.AreEqual(expectedIncrease, track.TotalHeightGain);
        }

        [Test]
        public void Ensure_TotalHeightGain_is_null_if_no_locations()
        {
            var track = this.GetTrackFromResponse(SampleResponse.Empty);
            Assert.IsNull(track.TotalHeightGain);
        }

        [Test]
        public void Ensure_TotalHeightGain_is_null_if_no_altitudes()
        {
            var xml = SampleResponse.Successful;
            xml.Descendants("LOCATION")
                .Elements("ALTITUDE")
                .Remove();

            var track = this.GetTrackFromResponse(xml);
            Assert.IsNull(track.TotalHeightGain);
        }

        [Test]
        public void Ensure_TotalHeightGain_is_zero_if_zero_altitudes()
        {
            var xml = SampleResponse.Successful;
            foreach (var altitude in xml.Descendants("ALTITUDE"))
            {
                altitude.Value = "0";
            }

            var track = this.GetTrackFromResponse(xml);
            Assert.AreEqual(0, track.TotalHeightGain);
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
