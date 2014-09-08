using Liath.ViewRanger.Exceptions;
using Liath.ViewRanger.RequestBuilders;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Liath.ViewRanger.Tests.RequestBuilderTests.GetLastPositionRequestTests
{
    /// <summary>
    /// These tests represent when the xml has come back from this.MakeRequest();
    /// </summary>
    [TestFixture]
    public class SuccessXmlDownloadedTests
    {
        [TestCase("NoLocations.xml")]
        [TestCase("TwoLocations.xml")]
        public void Throws_when_invalid_number_of_locations(string filename)
        {
            var request = new Mock<GetLastPositionRequest>(Guid.NewGuid().ToString());
            request.CallBase = true;
            request.Setup(x => x.MakeRequest(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(this.GetXDocument(filename));
            Assert.Throws<UnexpectedResponseException>(() => request.Object.Request());
        }

        private XDocument GetXDocument(string name)
        {
            return XDocument.Load(string.Concat(@"RequestBuilderTests\GetLastPositionRequestTests\RequestTests\SampleResponses\", name));
        }
    }
}
