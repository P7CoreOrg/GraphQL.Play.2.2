using System;
using GraphQL.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using TestServerFixture;

namespace UnitTestProject_TokenExchange
{
    [TestClass]
    public class TestServerContainer
    {
        public static GraphQLClientOptions GraphQLClientOptions;
        public static ITestServerFixture TestServerFixture { get; set; }
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            // Initalization code goes here
            TestServerFixture = new MyTestServerFixture();
            var endpoint = BuildGraphQLEndpoint();

            GraphQLClientOptions = new GraphQL.Client.GraphQLClientOptions()
            {
                HttpMessageHandler = TestServerFixture.MessageHandler,
                EndPoint = endpoint.Uri
            };
        }
        static UriBuilder BuildGraphQLEndpoint()
        {
            var endpoint = new UriBuilder(TestServerFixture.Client.BaseAddress.Scheme,
                TestServerFixture.Client.BaseAddress.Authority);
            endpoint.Path = "/api/v1/GraphQL";
            return endpoint;
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