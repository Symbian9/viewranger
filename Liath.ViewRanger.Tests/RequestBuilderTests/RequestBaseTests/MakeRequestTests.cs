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
    }
}
