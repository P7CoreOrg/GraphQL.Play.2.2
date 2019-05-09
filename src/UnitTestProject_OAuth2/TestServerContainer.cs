using System;
using GraphQL.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using TestServerFixture;

namespace UnitTestProject_OAuth2
{
    [TestClass]
    public class TestServerContainer
    {
        public static ITestServerFixture TestServerFixture { get; set; }
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            // Initalization code goes here
            TestServerFixture = new MyTestServerFixture();
         
        }
        [TestMethod]
        public void AssureFixture()
        {
            TestServerFixture.ShouldNotBeNull();
            var client = TestServerFixture.Client;
            client.ShouldNotBeNull();
        }
    }
}