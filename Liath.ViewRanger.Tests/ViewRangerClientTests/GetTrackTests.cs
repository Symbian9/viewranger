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
    }
}
