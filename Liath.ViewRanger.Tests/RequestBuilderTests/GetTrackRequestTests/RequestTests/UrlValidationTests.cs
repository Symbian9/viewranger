using Liath.ViewRanger.RequestBuilders;
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
    public class UrlValidationTests
    {
        public const string ApplicationKey = "AppKeyValue";
        public const string Username = "userValue";
        public const string Pin = "pinValue";
        public static DateTime FromDate = new DateTime(2010, 1, 1);
        public static DateTime ToDate = new DateTime(2014, 9, 9);
        public const int LimitValue = 25;

        [TestCase(RequestBase.KeyKey, ApplicationKey)]
        [TestCase(RequestBase.UsernameKey, Username)]
        [TestCase(RequestBase.PinKey, Pin)]
        [TestCase(GetTrackRequest.LimitKey, "25")]
        [TestCase(GetTrackRequest.FromKey, "2010-01-01 00:00:00")]
        [TestCase(GetTrackRequest.ToKey, "2014-09-09 00:00:00")]
        public void Check_parameters_are_correct(string key, string expectedValue)
        {
            var url = "http://www.someUrl.com";
            var request = new Mock<GetTrackRequest>(ApplicationKey, url);
            request.CallBase = true;
            request.Setup(x => x.CreateUrl(It.IsAny<RequestParameter[]>())).Callback<RequestParameter[]>(parameters =>
                {
                    var param = parameters.SingleOrDefault(p => p.Key == key);
                    Assert.IsNotNull(param);
                    Assert.AreEqual(expectedValue, param.Value);                    
                }).Returns(url);
            request.Setup(x => x.DownloadXml(url)).Returns(SampleResponse.Successful);

            request.Object.ForUser(Username, Pin)
                .From(FromDate)
                .To(ToDate)
                .Limit(LimitValue)
                .Request();
        }
    }
}
