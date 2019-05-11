using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using AppIdentity.Models;
using GraphQL.Client;
using GraphQL.Client.Http;
using GraphQL.Common.Request;
using IdentityModel;
using IdentityModel.Client;
using IdentityServer4;
using IdentityServer4.Contrib.CosmosDB.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using TestServerFixture;
using TokenExchange.Contracts;

namespace UnitTestProject_App_Identity
{

    [TestClass]
    public class AppIdentityTests
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
        public async Task Mint_AppId_Refresh_missing_subject()
        {
            var client = new TokenClient(
                _fixture.TestServer.BaseAddress + "connect/token",
                ClientId,
                _fixture.MessageHandler);

            Dictionary<string, string> paramaters = new Dictionary<string, string>()
            {
                {OidcConstants.TokenRequest.ClientId, ClientId},
                {OidcConstants.TokenRequest.ClientSecret, ClientSecret},
                {OidcConstants.TokenRequest.GrantType, ArbitraryNoSubjectExtensionGrant.Constants.ArbitraryNoSubject},
                {OidcConstants.TokenRequest.Scope, "nitro metal"},
                {
                    ArbitraryNoSubjectExtensionGrant.Constants.ArbitraryClaims,
                    $"{{'machineId': ['{Guid.NewGuid().ToString()}'],'appId': ['{Guid.NewGuid().ToString()}']}}"
                },
                {ArbitraryNoSubjectExtensionGrant.Constants.AccessTokenLifetime, "3600"},
                {ArbitraryIdentityExtensionGrant.Constants.IdTokenLifetime, "320000"}
            };
            var result = await client.RequestAsync(paramaters);

            result.ErrorDescription.ShouldBeNull();
            result.Error.ShouldBeNull();

            result.AccessToken.ShouldNotBeNullOrEmpty();


            using (var graphQLHttpClient =
                new GraphQL.Client.GraphQLClient(_graphQLClientOptions))
            {
                var appIdentityCreate = new GraphQLRequest(@"query q($input: appIdentityRefresh!) {
                                                                      appIdentityRefresh(input: $input){
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
                            id_token = result.AccessToken
                        }
                    }
                };

                var graphQLResponse = await graphQLHttpClient.PostAsync(appIdentityCreate);
                graphQLResponse.ShouldNotBeNull();
                var appIdentityResponse =
                    graphQLResponse
                        .GetDataFieldAs<AppIdentityResultModel>(
                            "appIdentityRefresh"); //data->appIdentityCreate is casted as AppIdentityResultModel
                appIdentityResponse.ShouldBeNull();
                graphQLResponse.Errors.ShouldNotBeNull();




            }
        }
        [TestMethod]
        public async Task Mint_AppId_Refresh_missing_appId()
        {
            var client = new TokenClient(
                _fixture.TestServer.BaseAddress + "connect/token",
                ClientId,
                _fixture.MessageHandler);

            Dictionary<string, string> paramaters = new Dictionary<string, string>()
            {
                {OidcConstants.TokenRequest.ClientId, ClientId},
                {OidcConstants.TokenRequest.ClientSecret, ClientSecret},
                {OidcConstants.TokenRequest.GrantType, ArbitraryNoSubjectExtensionGrant.Constants.ArbitraryIdentity},
                {
                    OidcConstants.TokenRequest.Scope, $"metal"
                },
                {
                    ArbitraryResourceOwnerExtensionGrant.Constants.Subject, "Ratt"
                },
                {
                    ArbitraryNoSubjectExtensionGrant.Constants.ArbitraryClaims,
                    $"{{'machineId': ['{Guid.NewGuid().ToString()}']}}"
                },
                {ArbitraryNoSubjectExtensionGrant.Constants.AccessTokenLifetime, "3600"},
                {ArbitraryIdentityExtensionGrant.Constants.IdTokenLifetime, "320000"}

            };
            var result = await client.RequestAsync(paramaters);
            result.ErrorDescription.ShouldBeNull();
            result.Error.ShouldBeNull();

            result.IdentityToken.ShouldNotBeNullOrEmpty();
            result.AccessToken.ShouldNotBeNullOrEmpty();


            using (var graphQLHttpClient =
                new GraphQL.Client.GraphQLClient(_graphQLClientOptions))
            {
                var appIdentityCreate = new GraphQLRequest(@"query q($input: appIdentityRefresh!) {
                                                                      appIdentityRefresh(input: $input){
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
                            id_token = result.IdentityToken
                        }
                    }
                };

                var graphQLResponse = await graphQLHttpClient.PostAsync(appIdentityCreate);
                graphQLResponse.ShouldNotBeNull();
                var appIdentityResponse =
                    graphQLResponse
                        .GetDataFieldAs<AppIdentityResultModel>(
                            "appIdentityRefresh"); //data->appIdentityCreate is casted as AppIdentityResultModel
                appIdentityResponse.ShouldBeNull();
                graphQLResponse.Errors.ShouldNotBeNull();




            }
        }
        [TestMethod]
        public async Task Mint_AppId_Refresh_missing_machineId()
        {
            var client = new TokenClient(
                _fixture.TestServer.BaseAddress + "connect/token",
                ClientId,
                _fixture.MessageHandler);

            Dictionary<string, string> paramaters = new Dictionary<string, string>()
            {
                {OidcConstants.TokenRequest.ClientId, ClientId},
                {OidcConstants.TokenRequest.ClientSecret, ClientSecret},
                {OidcConstants.TokenRequest.GrantType, ArbitraryNoSubjectExtensionGrant.Constants.ArbitraryIdentity},
                {
                    OidcConstants.TokenRequest.Scope, $"metal"
                },
                {
                    ArbitraryResourceOwnerExtensionGrant.Constants.Subject, "Ratt"
                },
                {
                    ArbitraryNoSubjectExtensionGrant.Constants.ArbitraryClaims,
                    $"{{'appId': ['{Guid.NewGuid().ToString()}']}}"
                },
                {ArbitraryNoSubjectExtensionGrant.Constants.AccessTokenLifetime, "3600"},
                {ArbitraryIdentityExtensionGrant.Constants.IdTokenLifetime, "320000"}

            };
            var result = await client.RequestAsync(paramaters);
            result.ErrorDescription.ShouldBeNull();
            result.Error.ShouldBeNull();

            result.IdentityToken.ShouldNotBeNullOrEmpty();
            result.AccessToken.ShouldNotBeNullOrEmpty();


            using (var graphQLHttpClient =
                new GraphQL.Client.GraphQLClient(_graphQLClientOptions))
            {
                var appIdentityCreate = new GraphQLRequest(@"query q($input: appIdentityRefresh!) {
                                                                      appIdentityRefresh(input: $input){
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
                            id_token = result.IdentityToken
                        }
                    }
                };

                var graphQLResponse = await graphQLHttpClient.PostAsync(appIdentityCreate);
                graphQLResponse.ShouldNotBeNull();
                var appIdentityResponse =
                    graphQLResponse
                        .GetDataFieldAs<AppIdentityResultModel>(
                            "appIdentityRefresh"); //data->appIdentityCreate is casted as AppIdentityResultModel
                appIdentityResponse.ShouldBeNull();
                graphQLResponse.Errors.ShouldNotBeNull();




            }
        }
        [TestMethod]
        public async Task success_graphQLDiscovery_GET()
        {

            using (var graphQLHttpClient =
                new GraphQL.Client.GraphQLClient(_graphQLClientOptions))
            {
                var graphQlRequest = new GraphQLRequest(@"query{
                                                              graphQLDiscovery{
                                                                graphQLEndpoints{
                                                                  name
                                                                  url
                                                                }
                                                              }
                                                            }");

                var graphQLResponse = await graphQLHttpClient.GetAsync(graphQlRequest);
                graphQLResponse.ShouldNotBeNull();

            }
        }
        /*
         * api/v1/GraphQL?query=query%20q(%24input%3A%20appIdentityBind!)%20%7BappIdentityBind(input%3A%20%24input)%7Bauthority%20expires_in%20id_token%7D%7D&variables=%7B%22input%22%3A%7B%22appId%22%3A%22NPM_IOS%22%2C%22machineId%22%3A%221234%22%7D%7D
         */
        [TestMethod]
        public async Task success_appIdentityCreate_GET()
        {

            using (var graphQLHttpClient =
                new GraphQL.Client.GraphQLClient(_graphQLClientOptions))
            {
                var appIdentityCreate = new GraphQLRequest(@"query q($input: appIdentityCreate!) {
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

                var graphQLResponse = await graphQLHttpClient.GetAsync(appIdentityCreate);
                graphQLResponse.ShouldNotBeNull();
                var appIdentityResponse = graphQLResponse.GetDataFieldAs<AppIdentityResultModel>("appIdentityCreate"); //data->appIdentityCreate is casted as AppIdentityResponse
                appIdentityResponse.ShouldNotBeNull();
                var handler = new JwtSecurityTokenHandler();
                var tokenS = handler.ReadToken(appIdentityResponse.id_token) as JwtSecurityToken;

                tokenS.ShouldNotBeNull();
            }
        }
        [TestMethod]
        public async Task success_appIdentityCreate_POST()
        {

            using (var graphQLHttpClient =
                new GraphQL.Client.GraphQLClient(_graphQLClientOptions))
            {
                var appIdentityCreate = new GraphQLRequest(@"query q($input: appIdentityCreate!) {
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

                var graphQLResponse = await graphQLHttpClient.PostAsync(appIdentityCreate);
                graphQLResponse.ShouldNotBeNull();
                var appIdentityResponse = graphQLResponse.GetDataFieldAs<AppIdentityResultModel>("appIdentityCreate"); //data->appIdentityCreate is casted as AppIdentityResponse
                appIdentityResponse.ShouldNotBeNull();
                var handler = new JwtSecurityTokenHandler();
                var tokenS = handler.ReadToken(appIdentityResponse.id_token) as JwtSecurityToken;

                tokenS.ShouldNotBeNull();
            }
        }
        [TestMethod]
        public async Task success_app_identity_bind_and_refresh_GET()
        {
            AppIdentityResultModel appIdentityResponse = null;
            using (var graphQLHttpClient =
                new GraphQL.Client.GraphQLClient(_graphQLClientOptions))
            {
                var appIdentityCreate = new GraphQLRequest(@"query q($input: appIdentityCreate!) {
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

                var graphQLResponse = await graphQLHttpClient.GetAsync(appIdentityCreate);
                appIdentityResponse = (AppIdentityResultModel)graphQLResponse.GetDataFieldAs<AppIdentityResultModel>("appIdentityCreate"); //data->appIdentityCreate is casted as AppIdentityResponse
                appIdentityResponse.ShouldNotBeNull();
                var handler = new JwtSecurityTokenHandler();
                var tokenS = handler.ReadToken(appIdentityResponse.id_token) as JwtSecurityToken;

                tokenS.ShouldNotBeNull();
            }

            /*
             * This next part fails because the validator calls out to authorities to get the public keys
             * The authority in the self case is under this TestServer and for some reason the discovery cache
             * cant reach this pretend http://localhost
             */
            using (var graphQLHttpClient =
                new GraphQL.Client.GraphQLClient(_graphQLClientOptions))
            {
                var appIdentityCreate = new GraphQLRequest(@"query q($input: appIdentityRefresh!) {
                                                                      appIdentityRefresh(input: $input){
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
                            id_token = appIdentityResponse.id_token
                        }
                    }
                };

                var graphQLResponse = await graphQLHttpClient.GetAsync(appIdentityCreate);
                graphQLResponse.ShouldNotBeNull();
                appIdentityResponse = graphQLResponse.GetDataFieldAs<AppIdentityResultModel>("appIdentityRefresh"); //data->appIdentityCreate is casted as AppIdentityResultModel
                appIdentityResponse.ShouldNotBeNull();
                var handler = new JwtSecurityTokenHandler();
                var tokenS = handler.ReadToken(appIdentityResponse.id_token) as JwtSecurityToken;

                tokenS.ShouldNotBeNull();
            }

        }
        [TestMethod]
        public async Task success_app_identity_bind_and_refresh()
        {
            AppIdentityResultModel appIdentityResponse = null;
            using (var graphQLHttpClient =
                new GraphQL.Client.GraphQLClient(_graphQLClientOptions))
            {
                var appIdentityCreate = new GraphQLRequest(@"query q($input: appIdentityCreate!) {
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

                var graphQLResponse = await graphQLHttpClient.PostAsync(appIdentityCreate);
                appIdentityResponse = (AppIdentityResultModel)graphQLResponse.GetDataFieldAs<AppIdentityResultModel>("appIdentityCreate"); //data->appIdentityCreate is casted as AppIdentityResponse
                appIdentityResponse.ShouldNotBeNull();
                var handler = new JwtSecurityTokenHandler();
                var tokenS = handler.ReadToken(appIdentityResponse.id_token) as JwtSecurityToken;

                tokenS.ShouldNotBeNull();
            }

            /*
             * This next part fails because the validator calls out to authorities to get the public keys
             * The authority in the self case is under this TestServer and for some reason the discovery cache
             * cant reach this pretend http://localhost
             */
            using (var graphQLHttpClient =
                new GraphQL.Client.GraphQLClient(_graphQLClientOptions))
            {
                var appIdentityCreate = new GraphQLRequest(@"query q($input: appIdentityRefresh!) {
                                                                      appIdentityRefresh(input: $input){
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
                            id_token = appIdentityResponse.id_token
                        }
                    }
                };

                var graphQLResponse = await graphQLHttpClient.PostAsync(appIdentityCreate);
                graphQLResponse.ShouldNotBeNull();
                appIdentityResponse = graphQLResponse.GetDataFieldAs<AppIdentityResultModel>("appIdentityRefresh"); //data->appIdentityCreate is casted as AppIdentityResultModel
                appIdentityResponse.ShouldNotBeNull();
                var handler = new JwtSecurityTokenHandler();
                var tokenS = handler.ReadToken(appIdentityResponse.id_token) as JwtSecurityToken;

                tokenS.ShouldNotBeNull();
            }

        }
        [TestMethod]
        public async Task success_app_identity_bind_and_token_exchange()
        {
            AppIdentityResultModel appIdentityResponse = null;
            using (var graphQLHttpClient =
                new GraphQL.Client.GraphQLClient(_graphQLClientOptions))
            {
                var appIdentityCreate = new GraphQLRequest(@"query q($input: appIdentityCreate!) {
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

                var graphQLResponse = await graphQLHttpClient.PostAsync(appIdentityCreate);
                appIdentityResponse = (AppIdentityResultModel)graphQLResponse.GetDataFieldAs<AppIdentityResultModel>("appIdentityCreate"); //data->appIdentityCreate is casted as AppIdentityResponse
                appIdentityResponse.ShouldNotBeNull();
                var handler = new JwtSecurityTokenHandler();
                var tokenS = handler.ReadToken(appIdentityResponse.id_token) as JwtSecurityToken;

                tokenS.ShouldNotBeNull();
            }

            /*
             * This next part fails because the validator calls out to authorities to get the public keys
             * The authority in the self case is under this TestServer and for some reason the discovery cache
             * cant reach this pretend http://localhost
             */
            using (var graphQLHttpClient =
                new GraphQL.Client.GraphQLClient(_graphQLClientOptions))
            {
                var appIdentityCreate = new GraphQLRequest(@"query q($input: tokenExchange!) {
                                                            tokenExchange(input: $input){
                                                                authority
                                                                access_token
                                                                token_type
                                                                httpHeaders{
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
                            tokens = new[] {
                                new {
                                    token = appIdentityResponse.id_token,
                                    tokenScheme = "self"
                                    }
                            },

                            exchange = "google-my-custom",
                            extras = new[] { "a", "b", "c" }
                        }
                    }
                };

                var graphQLResponse = await graphQLHttpClient.PostAsync(appIdentityCreate);
                graphQLResponse.ShouldNotBeNull();
                var bindResponse = (List<TokenExchangeResponse>)graphQLResponse.GetDataFieldAs<List<TokenExchangeResponse>>("tokenExchange"); //data->appIdentityCreate is casted as AppIdentityResponse
                bindResponse.ShouldNotBeNull();
                bindResponse.Count.ShouldBeGreaterThan(0);
                var handler = new JwtSecurityTokenHandler();
                var tokenS = handler.ReadToken(bindResponse[0].access_token) as JwtSecurityToken;

                tokenS.ShouldNotBeNull();
            }

        }

        [TestMethod]
        public async Task success_app_identity_bind_and_token_exchange_and_make_auth_call()
        {
            AppIdentityResultModel appIdentityResponse = null;
            using (var graphQLHttpClient =
                new GraphQL.Client.GraphQLClient(_graphQLClientOptions))
            {
                var appIdentityCreate = new GraphQLRequest(@"query q($input: appIdentityCreate!) {
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

                var graphQLResponse = await graphQLHttpClient.PostAsync(appIdentityCreate);
                appIdentityResponse =
                    (AppIdentityResultModel)graphQLResponse
                        .GetDataFieldAs<AppIdentityResultModel>("appIdentityCreate"); //data->appIdentityCreate is casted as AppIdentityResponse
                appIdentityResponse.ShouldNotBeNull();
                var handler = new JwtSecurityTokenHandler();
                var tokenS = handler.ReadToken(appIdentityResponse.id_token) as JwtSecurityToken;

                tokenS.ShouldNotBeNull();
            }

            /*
             * This next part fails because the validator calls out to authorities to get the public keys
             * The authority in the self case is under this TestServer and for some reason the discovery cache
             * cant reach this pretend http://localhost
             */
            string access_token = "";
            using (var graphQLHttpClient =
                new GraphQL.Client.GraphQLClient(_graphQLClientOptions))
            {
                var appIdentityCreate = new GraphQLRequest(@"query q($input: tokenExchange!) {
                                                            tokenExchange(input: $input){
                                                                authority
                                                                access_token
                                                                token_type
                                                                httpHeaders{
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
                            tokens = new[]
                            {
                                new
                                {
                                    token = appIdentityResponse.id_token,
                                    tokenScheme = "self"
                                }
                            },

                            exchange = "google-my-custom",
                            extras = new[] { "a", "b", "c" }
                        }
                    }
                };

                var graphQLResponse = await graphQLHttpClient.PostAsync(appIdentityCreate);
                graphQLResponse.ShouldNotBeNull();
                var bindResponse =
                    (List<TokenExchangeResponse>)graphQLResponse
                        .GetDataFieldAs<List<TokenExchangeResponse>>("tokenExchange"); //data->appIdentityCreate is casted as AppIdentityResponse
                bindResponse.ShouldNotBeNull();
                bindResponse.Count.ShouldBeGreaterThan(0);
                var handler = new JwtSecurityTokenHandler();
                var tokenS = handler.ReadToken(bindResponse[0].access_token) as JwtSecurityToken;

                tokenS.ShouldNotBeNull();
                access_token = bindResponse[0].access_token;


            }

            using (var graphQLHttpClient =
                new GraphQL.Client.GraphQLClient(_graphQLClientOptions))
            {
                var graphQlRequest = new GraphQLRequest(@"query{
                                                              authRequired{
                                                                claims{
                                                                  name
                                                                  value
                                                                }
                                                              }
                                                            }")
                {
                };
                graphQLHttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {access_token}");
                graphQLHttpClient.DefaultRequestHeaders.Add("x-authScheme", $"self-testserver");
                var graphQLResponse = await graphQLHttpClient.GetAsync(graphQlRequest);
                graphQLResponse.ShouldNotBeNull();





            }
        }

        [TestMethod]
        public async Task App_identity_bind_missing_argmuments_machineId()
        {

            using (var graphQLHttpClient =
                new GraphQL.Client.GraphQLClient(_graphQLClientOptions))
            {
                var appIdentityCreate = new GraphQLRequest(@"query q($input: appIdentityCreate!) {
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

                            appId = "myApp 001"
                        }
                    }
                };

                Should.Throw<GraphQLHttpException>(() =>
                {
                    graphQLHttpClient.PostAsync(appIdentityCreate).GetAwaiter().GetResult();
                });

            }
        }
        [TestMethod]
        public async Task App_identity_bind_missing_argmuments_appid()
        {

            using (var graphQLHttpClient =
                new GraphQL.Client.GraphQLClient(_graphQLClientOptions))
            {
                var appIdentityCreate = new GraphQLRequest(@"query q($input: appIdentityCreate!) {
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

                            machineId = "machineId 001"
                        }
                    }
                };

                Should.Throw<GraphQLHttpException>(() =>
                {
                    graphQLHttpClient.PostAsync(appIdentityCreate).GetAwaiter().GetResult();
                });

            }
        }
    }
}
