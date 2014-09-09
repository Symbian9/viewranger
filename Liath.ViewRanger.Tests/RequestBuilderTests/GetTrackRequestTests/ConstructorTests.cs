using Liath.ViewRanger.RequestBuilders;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liath.ViewRanger.Tests.RequestBuilderTests.GetTrackRequestTests
{
    [TestFixture]
    public class ConstructorTests
    {
        [Test]
        public void Ensure_does_not_throw_when_all_parameters_supplied()
        {
            Assert.DoesNotThrow(() => new GetTrackRequest(Guid.NewGuid().ToString()));
        }

        [Test]
        public void Ensure_throw_when_applicationKey_not_supplied()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new GetTrackRequest(null));
            Assert.AreEqual("applicationKey", ex.ParamName);
        }

        [Test]
        public void Check_service_is_correct()
        {
            Assert.AreEqual("getBBPositions", new GetTrackRequest(Guid.NewGuid().ToString()).Service);
        }
    }
}
