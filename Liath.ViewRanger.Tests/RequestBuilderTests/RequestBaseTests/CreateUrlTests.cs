using Liath.ViewRanger.RequestBuilders;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liath.ViewRanger.Tests.RequestBuilderTests.RequestBaseTests
{
    [TestFixture]
    public class CreateUrlTests
    {
        [Test]
        public void Check_url_is_created_correctly()
        {
            var key = "TheKey";
            var baseUrl = "http://www.somewhere.com/";
            var response = new Mock<GetLastPositionRequest>(key);
            response.CallBase = true;
            response.Setup(x => x.BaseAddress).Returns(baseUrl);

            var url = response.Object.CreateUrl(new RequestParameter("a", "1"), new RequestParameter("b", "2"));

            Assert.AreEqual(string.Concat(baseUrl, "?a=1&b=2"), url);
        }
    }
}
