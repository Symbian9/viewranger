using Liath.ViewRanger.Exceptions;
using Liath.ViewRanger.RequestBuilders;
using Liath.ViewRanger.Tests.RequestBuilderTests.GetLastPositionRequestTests.RequestTests.SampleResponses;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Liath.ViewRanger.Tests.RequestBuilderTests.RequestBaseTests
{
    [TestFixture]
    public class MakeRequestTests
    {
        [Test]
        public void Ensure_MakeRequest_downloads_xml()
        {
            var urlToCall = Guid.NewGuid().ToString();
            var xml = SampleResponse.Successful;
            var request = new Mock<GetLastPositionRequest>(Guid.NewGuid().ToString());
            request.CallBase = true;
            request.Setup(x => x.CreateUrl(It.IsAny<RequestParameter[]>())).Returns(urlToCall);
            request.Setup(x => x.DownloadXml(urlToCall)).Returns(xml).Verifiable();

            request.Object.ForUser(Guid.NewGuid().ToString(), Guid.NewGuid().ToString()).Request();

            request.Verify(x => x.DownloadXml(urlToCall), Times.Once());
        }

        [Test]
        public void Ensure_error_messages_are_thrown_as_exceptions()
        {
            var xml = SampleResponse.Error;
            var request = new Mock<GetLastPositionRequest>(Guid.NewGuid().ToString());
            request.CallBase = true;
            request.Setup(x => x.DownloadXml(It.IsAny<string>())).Returns(xml);
            var preparedRequest = request.Object.ForUser(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

            var ex = Assert.Throws<FailedRequestException>(() => preparedRequest.Request());
            Assert.AreEqual("ViewRanger was unable to complete the request", ex.Message);
            Assert.AreEqual("1", ex.ViewRangerCode);
            Assert.AreEqual("Invalid API key", ex.ViewRangerMessage);
        }
    }
}
