using Liath.ViewRanger.Exceptions;
using Liath.ViewRanger.RequestBuilders;
using Liath.ViewRanger.Responses;
using Liath.ViewRanger.Tests.RequestBuilderTests.GetLastPositionRequestTests.RequestTests.SampleResponses;
using Liath.ViewRanger.Tests.RequestBuilderTests.RequestBaseTests;
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
    public class ErrorResponseTests : ErrorTesting<IGetLastPositionRequest, Location>
    {
        // This class ensures that all GetTrackRequest error messages are checked, the BaseClass actually does the implementation
        // of the tests - after all the same codes should produce the same errors for both!

        public override IGetLastPositionRequest CreateRequest(string code)
        {
            var xml = SampleResponse.Error;
            xml.Descendants("CODE").Single().Value = code;
            xml.Descendants("MESSAGE").Single().Value = Guid.NewGuid().ToString();
            var request = new Mock<GetLastPositionRequest>(Guid.NewGuid().ToString());
            request.CallBase = true;
            request.Setup(x => x.DownloadXml(It.IsAny<string>())).Returns(xml);
            return request.Object.ForUser(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
        }

        [Test]
        public void Ensure_internal_error_throws_ClientException()
        {
            var request = new Mock<GetLastPositionRequest>(Guid.NewGuid().ToString());
            request.CallBase = true;
            request.Setup(x => x.DownloadXml(It.IsAny<string>())).Throws(new NullReferenceException());
            var readyToRequest = request.Object.ForUser(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            Assert.Throws<ClientException>(() => readyToRequest.Request());
        }
    }
}
