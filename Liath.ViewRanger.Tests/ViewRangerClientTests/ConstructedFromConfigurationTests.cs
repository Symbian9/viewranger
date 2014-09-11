using Liath.ViewRanger.Configuration;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Liath.ViewRanger.Tests.ViewRangerClientTests
{
    [TestFixture]
    public class ConstructedFromConfigurationTests
    {
        // This isn't really an example of good unit testing, we're checking private variables and doing all kinds of crazy inheritance
        // Issues faced
        //      - Can't swap out a config file for each test, therefore obviously we need to inject them
        //      - Can't stub protected methods (meaning unless we reveal LoadConfigurationSection to the library consumer) we can't replace it
        //      - BaseConstructors are called before the child one, meaning we can't pass a config in in the constructor to be used by the base
        //      - We're testing the constructor we're mocking we get TargetInvocationException with the actual exception in the InnerException property       

        // If someone can solve all of the above they're welcome to rewrite this file!

        [Test]
        public void Check_no_config_throws()
        {
            var ex = Assert.Throws<ConfigurationErrorsException>(() => this.CreateClient(null));
        }

        [Test]
        public void Check_config_no_key_throws()
        {
            var config = new ViewRangerConfigurationSection
            {
                BaseAddress = new AddressElement
                {
                    Url = Guid.NewGuid().ToString()
                }
            };

            var ex = Assert.Throws<ConfigurationErrorsException>(() => this.CreateClient(config));
        }

        [Test]
        public void Check_config_no_address_does_not_throw()
        {
            var config = new ViewRangerConfigurationSection
            {
                ApplicationKey = new ApplicationKeyElement
                {
                    Key = Guid.NewGuid().ToString()
                }
            };
            Assert.DoesNotThrow(() => this.CreateClient(config));
        }

        [Test]
        public void Check_config_no_address_defaults()
        {
            var config = new ViewRangerConfigurationSection
            {
                ApplicationKey = new ApplicationKeyElement
                {
                    Key = Guid.NewGuid().ToString()
                }
            };

            var client = this.CreateClient(config);
            this.AssertPrivateVariable(client, "_baseAddress", ViewRangerClient.DefaultApiBaseAddress);
        }

        [Test]
        public void Check_baseAddress_is_set()
        {
            var address = Guid.NewGuid().ToString();
            var config = new ViewRangerConfigurationSection
            {
                ApplicationKey = new ApplicationKeyElement
                {
                    Key = Guid.NewGuid().ToString()
                },
                BaseAddress = new AddressElement
                {
                    Url = address
                }
            };
            var client = this.CreateClient(config);
            this.AssertPrivateVariable(client, "_baseAddress", address);
        }

        [Test]
        public void Check_applicationKey_is_set()
        {
            var key = Guid.NewGuid().ToString();
            var config = new ViewRangerConfigurationSection
            {
                ApplicationKey = new ApplicationKeyElement
                {
                    Key = key
                }
            };
            var client = this.CreateClient(config);
            this.AssertPrivateVariable(client, "_applicationKey", key);
        }

        /// <summary>
        /// Create a ViewRanger client which will load the given configuration section
        /// </summary>
        /// <param name="config">The config section to load</param>
        public ViewRangerClient CreateClient(ViewRangerConfigurationSection config)
        {
            try
            {
                var client = new Mock<TestingViewRangerClient>();
                client.Setup(x => x.PublicConfigurationLoad()).Returns(config);
                client.CallBase = true;
                return client.Object;
            }
            catch(TargetInvocationException tiEx) // if we get errors unwrap them from the Mock/Reflection exception
            {
                throw tiEx.InnerException;
            }
            catch(Exception)
            {
                throw; // anything else throw it!
            }
        }

        /// <summary>
        /// Asserts that the private variable has been set correctly
        /// </summary>
        public void AssertPrivateVariable(ViewRangerClient client, string variableName, string expectedValue)
        {
            var currentValue = typeof(ViewRangerClient).GetField(variableName,
                        BindingFlags.NonPublic |
                        BindingFlags.Instance)
                    .GetValue(client);
            Assert.AreEqual(expectedValue, currentValue);
        }
    }

    /// <summary>
    /// This class allows us to pass in the configuration sections we want rather than making the protected LoadConfigurationSection method public
    /// </summary>
    public class TestingViewRangerClient : ViewRangerClient
    {
        public TestingViewRangerClient()
            : base()
        {

        }

        /// <summary>
        /// Overrides the LoadConfigurationSection and returns the PublicConfigurationLoad which can be stubbed
        /// </summary>
        protected override ViewRangerConfigurationSection LoadConfigurationSection()
        {
            return this.PublicConfigurationLoad();
        }

        /// <summary>
        /// A method designed to be overridden to allow us to define our own config sections
        /// </summary>
        public virtual ViewRangerConfigurationSection PublicConfigurationLoad()
        {
            throw new NotImplementedException("This method should be stubbed");
        }
    }
}
