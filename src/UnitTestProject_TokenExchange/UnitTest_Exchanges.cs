using System;
using System.Collections.Generic;
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

        async Task<AppIdentityResultModel> FetchAppIdentityAsync()
        {
            string id_token = "";
            var graphQLHttpClient =
                new GraphQL.Client.GraphQLClient(_graphQLClientOptions);
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
            var appIdentityResponse =
                graphQLResponse
                    .GetDataFieldAs<AppIdentityResultModel>(
                        "appIdentityCreate"); //data->appIdentityCreate is casted as AppIdentityResponse
            appIdentityResponse.ShouldNotBeNull();
            return appIdentityResponse;
        }

        [TestMethod]
        public async Task success_app_identity_bind_and_exchange_selfvalidator()
        {
            string id_token = "";
            var appIdentityResponse = await FetchAppIdentityAsync();
            appIdentityResponse.ShouldNotBeNull();
            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadToken(appIdentityResponse.id_token) as JwtSecurityToken;

            tokenS.ShouldNotBeNull();
            id_token = appIdentityResponse.id_token;


            var graphQLHttpClient =
                new GraphQL.Client.GraphQLClient(_graphQLClientOptions);
            var graphQlRequest = new GraphQLRequest(
                @"query q($input: tokenExchange!) {
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
                            new
                            {
                                token = id_token,
                                tokenScheme = "self"
                            }
                        }
                    }
                }
            };

            var graphQLResponse = await graphQLHttpClient.PostAsync(graphQlRequest);
            graphQLResponse.ShouldNotBeNull();
            graphQLResponse.Errors.ShouldBeNull();


        }
        [TestMethod]
        public async Task success_app_identity_bind_and_exchange()
        {
            string id_token = "";
            var appIdentityResponse = await FetchAppIdentityAsync();
            appIdentityResponse.ShouldNotBeNull();
            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadToken(appIdentityResponse.id_token) as JwtSecurityToken;

            tokenS.ShouldNotBeNull();
            id_token = appIdentityResponse.id_token;


            var graphQLHttpClient =
                new GraphQL.Client.GraphQLClient(_graphQLClientOptions);
            var graphQlRequest = new GraphQLRequest(
                @"query q($input: tokenExchange!) {
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
                            new
                            {
                                token = id_token,
                                tokenScheme = "self-testserver"
                            }
                        }
                    }
                }
            };

            var graphQLResponse = await graphQLHttpClient.PostAsync(graphQlRequest);
            graphQLResponse.ShouldNotBeNull();
            graphQLResponse.Errors.ShouldBeNull();


        }
        [TestMethod]
        public async Task fail_Bad_token_exchange()
        {
            string id_token = Guid.NewGuid().ToString();



            var graphQLHttpClient =
                new GraphQL.Client.GraphQLClient(_graphQLClientOptions);
            var graphQlRequest = new GraphQLRequest(
                @"query q($input: tokenExchange!) {
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
                            new
                            {
                                token = id_token,
                                tokenScheme = "self-testserver"
                            }
                        }
                    }
                }
            };

            var graphQLResponse = await graphQLHttpClient.PostAsync(graphQlRequest);

            graphQLResponse.Errors.ShouldNotBeNull();


        }
        class TokenScheme
        {
            public string token { get; set; }
            public string tokenScheme { get; set; }
        }
        [TestMethod]
        public async Task fail_missing_token_exchange()
        {
            string id_token = Guid.NewGuid().ToString();

            var tokens = new List<TokenScheme>();

            var graphQLHttpClient =
                new GraphQL.Client.GraphQLClient(_graphQLClientOptions);
            var graphQlRequest = new GraphQLRequest(
                @"query q($input: tokenExchange!) {
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
                        tokens = tokens
                    }
                }
            };

            var graphQLResponse = await graphQLHttpClient.PostAsync(graphQlRequest);

            graphQLResponse.Errors.ShouldNotBeNull();


        }
        [TestMethod]
        public async Task fail_null_token_exchange()
        {
            string id_token = Guid.NewGuid().ToString();

            var tokens = new List<TokenScheme>();

            var graphQLHttpClient =
                new GraphQL.Client.GraphQLClient(_graphQLClientOptions);
            var graphQlRequest = new GraphQLRequest(
                @"query q($input: tokenExchange!) {
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
                        }
                    }
                }
            };

            var graphQLResponse = await graphQLHttpClient.PostAsync(graphQlRequest);

            graphQLResponse.Errors.ShouldNotBeNull();


        }
    }
}
