using Liath.ViewRanger.RequestBuilders;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liath.ViewRanger.Tests.RequestBuilderTests.GetLastPositionRequestTests
{
    [TestFixture]
    public class ForUserTests
    {
        [Test]
        public void Throws_when_username_is_null()
        {
            var request = new GetLastPositionRequest(Guid.NewGuid().ToString());
            var ex = Assert.Throws<ArgumentNullException>(() =>
                {
                    request.ForUser(null, Guid.NewGuid().ToString());
                });
            Assert.AreEqual("username", ex.ParamName);
        }

        [Test]
        public void Throws_when_pin_is_null()
        {
            var request = new GetLastPositionRequest(Guid.NewGuid().ToString());
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                request.ForUser(Guid.NewGuid().ToString(), null);
            });
            Assert.AreEqual("pin", ex.ParamName);
        }

        [Test]
        public void Ensure_returns_itself()
        {
            var request = new GetLastPositionRequest(Guid.NewGuid().ToString());
            var nextRequestObject = request.ForUser(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            Assert.AreSame(request, nextRequestObject);
        }

        [Test]
        public void Check_username_is_set()
        {
            var username = Guid.NewGuid().ToString();
            var request = new GetLastPositionRequest(Guid.NewGuid().ToString());
            var nextRequestObject = request.ForUser(username, Guid.NewGuid().ToString());
            Assert.AreEqual(username, ((GetLastPositionRequest)nextRequestObject).Username);
        }

        [Test]
        public void Check_pin_is_set()
        {
            var pin = Guid.NewGuid().ToString();
            var request = new GetLastPositionRequest(Guid.NewGuid().ToString());
            var nextRequestObject = request.ForUser(Guid.NewGuid().ToString(), pin);
            Assert.AreEqual(pin, ((GetLastPositionRequest)nextRequestObject).Pin);
        }
    }
}
