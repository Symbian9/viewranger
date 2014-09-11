using Liath.ViewRanger.RequestBuilders;
using Liath.ViewRanger.Tests.RequestBuilderTests.GetLastPositionRequestTests.RequestTests.SampleResponses;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liath.ViewRanger.Tests.RequestBuilderTests.GetLastPositionRequestTests.RequestTests
{
    [TestFixture]
    public class UrlValidationTests
    {
        public const string ApplicationKey = "AppKeyValue";
        public const string Username = "userValue";
        public const string Pin = "pinValue";

        [TestCase(RequestBase.KeyKey, ApplicationKey)]
        [TestCase(RequestBase.UsernameKey, Username)]
        [TestCase(RequestBase.PinKey, Pin)]
        public void Check_parameters_are_correct(string key, string expectedValue)
        {
            var url = "http://www.someUrl.com";
            var request = new Mock<GetLastPositionRequest>(ApplicationKey, url);
            request.CallBase = true;
            request.Setup(x => x.CreateUrl(It.IsAny<RequestParameter[]>())).Callback<RequestParameter[]>(parameters =>
                {
                    var param = parameters.SingleOrDefault(p => p.Key == key);
                    Assert.IsNotNull(param);
                    Assert.AreEqual(expectedValue, param.Value);
                }).Returns(url);
            request.Setup(x => x.DownloadXml(url)).Returns(SampleResponse.Successful);

            request.Object.ForUser(Username, Pin).Request();
        }
    }
}
