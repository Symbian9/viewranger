using Liath.ViewRanger.RequestBuilders;
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

        [TestCase(RequestBase.ServiceKey, ApplicationKey)]
        [TestCase(RequestBase.UsernameKey, Username)]
        [TestCase(RequestBase.PinKey, Pin)]
        public void Check_parameters_are_correct(string key, string expectedValue)
        {
            var request = new Mock<GetLastPositionRequest>(ApplicationKey);
            request.Setup(x => x.CreateUrl(It.IsAny<RequestParameter[]>())).Callback<RequestParameter[]>(parameters =>
                {
                    var param = parameters.SingleOrDefault(p => p.Key == key);
                    Assert.IsNotNull(param);
                    Assert.AreEqual(expectedValue, param.Value);
                });

            request.Object.ForUser(Username, Pin).Request();
        }
    }
}
