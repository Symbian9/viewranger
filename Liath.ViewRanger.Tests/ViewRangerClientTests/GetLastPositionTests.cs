using Liath.ViewRanger.RequestBuilders;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liath.ViewRanger.Tests.ViewRangerClientTests
{
    [TestFixture]
    public class GetLastPositionTests
    {
        [Test]
        public void Request_Created_Correctly()
        {
            var client = new ViewRangerClient(Guid.NewGuid().ToString());
            Assert.IsNotNull(client.GetLastPosition());
        }

        [Test]
        public void Request_Created_With_ApplicationID()
        {
            var key = Guid.NewGuid().ToString();
            var client = new ViewRangerClient(key);
            Assert.AreEqual(key, ((GetLastPositionRequest)client.GetLastPosition()).ApplicationKey);
        }

        [Test]
        public void Request_Created_With_BaseAddress()
        {
            var address = Guid.NewGuid().ToString();
            var client = new ViewRangerClient(Guid.NewGuid().ToString(), address);
            Assert.AreEqual(address, ((GetLastPositionRequest)client.GetLastPosition()).BaseAddress);
        }
    }
}
