using NUnit.Framework;
using System.Collections.Generic;
using User.Domain;
using User.Service.Extensions;

namespace User.Tests
{
    public class DictionaryExtensionTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Given_int_should_Execute_successfully()
        {
            var user = new UserInfo { Id = 2 };
            var dict = new Dictionary<string, string>();
            dict.Add("id", "2");
            var exp = dict.GetExpression<UserInfo>();
            var result = exp.Compile().Invoke(user);

            Assert.IsTrue(result);
        }

        [Test]
        public void Given_string_should_Execute_successfully()
        {
            var user = new UserInfo { FirstName = "name" };
            var dict = new Dictionary<string, string>();
            dict.Add("firstName", "name");
            var exp = dict.GetExpression<UserInfo>();
            var result = exp.Compile().Invoke(user);

            Assert.IsTrue(result);
        }

        [Test]
        public void Given_boolean_should_Execute_successfully()
        {
            var user = new UserInfo {  Status = true };
            var dict = new Dictionary<string, string>();
            dict.Add("Status","true");
            var exp = dict.GetExpression<UserInfo>();
            var result = exp.Compile().Invoke(user);

            Assert.IsTrue(result);
        }
    }
}