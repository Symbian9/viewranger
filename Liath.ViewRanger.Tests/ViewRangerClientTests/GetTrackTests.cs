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
    public class GetTrackTests
    {
        [Test]
        public void Request_Created_Correctly()
        {
            var client = new ViewRangerClient(Guid.NewGuid().ToString());
            Assert.IsNotNull(client.GetTrack());
        }

        [Test]
        public void Request_Created_With_ApplicationID()
        {
            var key = Guid.NewGuid().ToString();
            var client = new ViewRangerClient(key);
            Assert.AreEqual(key, ((GetTrackRequest)client.GetTrack()).ApplicationKey);
        }

        [Test]
        public void Request_Created_With_BaseAddress()
        {
            var address = Guid.NewGuid().ToString();
            var client = new ViewRangerClient(Guid.NewGuid().ToString(), address);
            Assert.AreEqual(address, ((GetTrackRequest)client.GetTrack()).BaseAddress);
        }
    }
}
