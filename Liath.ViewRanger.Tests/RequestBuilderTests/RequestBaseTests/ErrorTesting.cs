using Liath.ViewRanger.Exceptions;
using Liath.ViewRanger.RequestBuilders;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liath.ViewRanger.Tests.RequestBuilderTests.RequestBaseTests
{
    public abstract class ErrorTesting<TRequestType, TReturnType> where TRequestType : IRequest<TReturnType>
    {
        public abstract TRequestType CreateRequest(string code);

        [TestCase("1")]
        [TestCase("2")]
        [TestCase("3")]
        [TestCase("5")]
        public void Ensure_code_throws_ApiKeyRejectedException(string code)
        {
            var request = this.CreateRequest(code);
            Assert.Throws<ApiKeyRejectedException>(() => request.Request());
        }

        [TestCase("10")]
        [TestCase("11")]
        public void Ensure_code_throws_InvalidUserCredentials(string code)
        {
            var request = this.CreateRequest(code);
            Assert.Throws<InvalidUserCredentialsException>(() => request.Request());
        }

        [TestCase("4")]
        [TestCase("8")]
        [TestCase("9")]
        public void Ensure_code_throws_MalformedRequestException(string code)
        {
            var request = this.CreateRequest(code);
            Assert.Throws<MalformedRequestException>(() => request.Request());
        }

        [TestCase("6")]
        public void Ensure_code_throws_InternalViewRangerException(string code)
        {
            var request = this.CreateRequest(code);
            Assert.Throws<InternalViewRangerException>(() => request.Request());
        }

        [TestCase("15")] // A code we don't recognise
        public void Ensure_code_throws_FailedRequestException(string code)
        {
            var request = this.CreateRequest(code);
            Assert.Throws<FailedRequestException>(() => request.Request());
        }        
    }
}
