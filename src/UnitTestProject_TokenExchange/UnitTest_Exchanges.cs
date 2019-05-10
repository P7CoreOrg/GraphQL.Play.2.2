using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using AppIdentity.Models;
using GraphQL.Client;
using GraphQL.Common.Request;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using TestServerFixture;

namespace UnitTestProject_TokenExchange
{
    [TestClass]
    public class UnitTest_Exchanges
    {
        public string ClientId => "arbitrary-resource-owner-client";
        public string ClientSecret => "secret";

        public static GraphQLClientOptions _graphQLClientOptions;
        public static ITestServerFixture _fixture;

        [ClassInitialize]
        public static void IntializeClass(TestContext testContext)
        {
            _fixture = TestServerContainer.TestServerFixture;
            _graphQLClientOptions = TestServerContainer.GraphQLClientOptions;
        }
        [TestMethod]
        public async Task success_app_identity_bind_and_exchange()
        {
            string id_token = "";
            using (var graphQLHttpClient =
                new GraphQL.Client.GraphQLClient(_graphQLClientOptions))
            {
                var graphQlRequest = new GraphQLRequest(@"query q($input: appIdentityCreate!) {
                          appIdentityCreate(input: $input){
                            authority
                              expires_in
                              id_token
                            }
                        }")
                {

                    OperationName = null,
                    Variables = new
                    {
                        input = new
                        {
                            appId = "myApp 001",
                            machineId = "machineId 001"
                        }
                    }
                };

                var graphQLResponse = await graphQLHttpClient.PostAsync(graphQlRequest);
                graphQLResponse.ShouldNotBeNull();
                var appIdentityResponse = graphQLResponse.GetDataFieldAs<AppIdentityResultModel>("appIdentityCreate"); //data->appIdentityCreate is casted as AppIdentityResponse
                appIdentityResponse.ShouldNotBeNull();
                var handler = new JwtSecurityTokenHandler();
                var tokenS = handler.ReadToken(appIdentityResponse.id_token) as JwtSecurityToken;

                tokenS.ShouldNotBeNull();
                id_token = appIdentityResponse.id_token;

            }


            using (var graphQLHttpClient =
                new GraphQL.Client.GraphQLClient(_graphQLClientOptions))
            {
                var graphQlRequest = new GraphQLRequest(@"query q($input: tokenExchange!) {
	                                                                    tokenExchange(input: $input){
			                                                                    authority
                                                                          access_token
                                                                          token_type
                                                                          httpHeaders
                                                                          {
				                                                                    name
                                                                            value
 			                                                                    }
                                                                        }
                                                                    }")
                {

                    OperationName = null,
                    Variables = new
                    {
                        input = new
                        {
                            exchange = "google-my-custom",
                            extras = new string[]
                            {
                                "a", "b", "c"
                            },
                            tokens = new[]
                            {
                                new {token= id_token,tokenScheme="self"}
                            }
                        }
                    }
                };

                var graphQLResponse = await graphQLHttpClient.PostAsync(graphQlRequest);
                graphQLResponse.ShouldNotBeNull();
                graphQLResponse.Errors.ShouldBeNull();


            }
        }
    }
}
