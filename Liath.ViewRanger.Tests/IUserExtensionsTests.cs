using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liath.ViewRanger.Tests
{
    [TestFixture]
    public class IUserExtensionsTests
    {
        [Test]
        public void Ensure_false_when_username_null()
        {
            Assert.IsFalse(new UserAccount(null, Guid.NewGuid().ToString()).HasBuddyBeaconCredentials());
        }

        [Test]
        public void Ensure_false_when_pin_null()
        {
            Assert.IsFalse(new UserAccount(Guid.NewGuid().ToString(), null).HasBuddyBeaconCredentials());
        }

        [Test]
        public void Ensure_false_when_username_white()
        {
            Assert.IsFalse(new UserAccount("             ", Guid.NewGuid().ToString()).HasBuddyBeaconCredentials());
        }

        [Test]
        public void Ensure_false_when_pin_white()
        {
            Assert.IsFalse(new UserAccount(Guid.NewGuid().ToString(), "    ").HasBuddyBeaconCredentials());
        }

        [Test]
        public void Ensure_false_when_both_null()
        {
            Assert.IsFalse(new UserAccount(null, null).HasBuddyBeaconCredentials());
        }

        [Test]
        public void Ensure_false_when_both_populated()
        {
            Assert.IsTrue(new UserAccount(Guid.NewGuid().ToString(), Guid.NewGuid().ToString()).HasBuddyBeaconCredentials());
        }


        /// <summary>
        /// Quick class to test the IUser, saves mocking it over and over again
        /// </summary>
        private class UserAccount : IUser
        {
            private string _username;
            private string _pin;

            public UserAccount(string username, string pin)
            {
                _username = username;
                _pin = pin;
            }

            public string BuddyBeaconUsername
            {
                get { return _username; }
            }

            public string BuddyBeaconPin
            {
                get { return _pin; }
            }
        }
    }
}
