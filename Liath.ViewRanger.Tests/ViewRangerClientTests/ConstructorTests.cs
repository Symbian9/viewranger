using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liath.ViewRanger.Tests.ViewRangerClientTests
{
    [TestFixture]
    public class ConstructorTests
    {
        [Test]
        public void Does_not_throw_when_applicationKey_supplied()
        {
            Assert.DoesNotThrow(() => new ViewRangerClient(Guid.NewGuid().ToString()));
        }

        [Test]
        public void Throw_when_applicationKey_not_supplied()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new ViewRangerClient(null));
            Assert.AreEqual("applicationKey", ex.ParamName);
        }
    }
}
