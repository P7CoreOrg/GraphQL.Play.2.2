using GraphQL.Client;
using GraphQL.Common.Request;
using Newtonsoft.Json;
using Shouldly;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AppIdentity.Models;
using DiscoveryHub.Contracts;
using DiscoveryHub.Contracts.Models;
using DiscoveryHub.Query;
using FakeItEasy;
using FakeItEasyCaptures.Helpers;
using GraphQL;
using GraphQL.Client;
using GraphQL.Client.Http;
using GraphQL.Types;
using IdentityModel;
using IdentityModel.Client;

using IdentityTokenExchangeGraphQL.Models;
using Xunit;
using TokenExchange.Contracts;

namespace XUnitServer_App_Identity
{
    public class ClientCredentialsResponse
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string token_type { get; set; }

    }
    class GraphQLDiscoveryModel
    {
        public List<GraphQLEndpoint> graphQLEndpoints { get; set; }
    }
    public class AppIdentityTests : IClassFixture<MyTestServerFixture>
    {
        public string ClientId => "arbitrary-resource-owner-client";
        public string ClientSecret => "secret";
        private readonly MyTestServerFixture _fixture;
        private GraphQLClientOptions _graphQLClientOptions;

        public AppIdentityTests(MyTestServerFixture fixture)
        {
            _fixture = fixture;
            var endpoint = BuildGraphQLEndpoint();

            _graphQLClientOptions = new GraphQL.Client.GraphQLClientOptions()
            {
                HttpMessageHandler = _fixture.MessageHandler,
                EndPoint = endpoint.Uri
            };
        }
        UriBuilder BuildGraphQLEndpoint()
        {
            var endpoint = new UriBuilder(_fixture.Client.BaseAddress.Scheme,
                _fixture.Client.BaseAddress.Authority);
            endpoint.Path = "/api/v1/GraphQL";
            return endpoint;
        }
        [Fact]
        public void AssureFixture()
        {
            _fixture.ShouldNotBeNull();
            var client = _fixture.Client;
            client.ShouldNotBeNull();
        }

        [Fact]
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
        [Fact]
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
        [Fact]
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
        [Fact]
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

                var graphQLDiscoveryModel = (GraphQLDiscoveryModel)graphQLResponse.GetDataFieldAs<GraphQLDiscoveryModel>("graphQLDiscovery"); //data->graphQLDiscovery is casted as GraphQLDiscoveryModel
                graphQLDiscoveryModel.ShouldNotBeNull();
                graphQLDiscoveryModel.graphQLEndpoints.ShouldNotBeNull();
                graphQLDiscoveryModel.graphQLEndpoints.Count.ShouldBeGreaterThan(0);

            }
        }

        /*
         * api/v1/GraphQL?query=query%20q(%24input%3A%20appIdentityBind!)%20%7BappIdentityBind(input%3A%20%24input)%7Bauthority%20expires_in%20id_token%7D%7D&variables=%7B%22input%22%3A%7B%22appId%22%3A%22NPM_IOS%22%2C%22machineId%22%3A%221234%22%7D%7D
         */
        [Fact]
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
        [Fact]
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
        [Fact]
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
        [Fact]
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
        [Fact]
        public async Task success_app_identity_bind_and_token_exchange()
        {
            AppIdentityResultModel appIdentityResponse = null;
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
                var graphQlRequest = new GraphQLRequest(@"query q($input: tokenExchange!) {
                                                            tokenExchange(input: $input){
                                                                   accessToken{
                                                                        hint
                                                                        authority
                                                                        expires_in
                                                                        access_token
                                                                        refresh_token
                                                                        token_type
                                                                        httpHeaders
                                                                        {
	                                                                        name
                                                                            value
                                                                        }
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

                var graphQLResponse = await graphQLHttpClient.PostAsync(graphQlRequest);
                graphQLResponse.ShouldNotBeNull();
                var bindResponse = (List<TokenExchangeResponse>)graphQLResponse.GetDataFieldAs<List<TokenExchangeResponse>>("tokenExchange"); //data->appIdentityCreate is casted as AppIdentityResponse
                bindResponse.ShouldNotBeNull();
                bindResponse.Count.ShouldBeGreaterThan(0);
                var handler = new JwtSecurityTokenHandler();
                var tokenS = handler.ReadToken(bindResponse[0].accessToken.access_token) as JwtSecurityToken;

                tokenS.ShouldNotBeNull();
            }

        }

        [Fact]
        public async Task success_app_identity_bind_and_token_exchange_and_make_auth_call()
        {
            AppIdentityResultModel appIdentityResponse = null;
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
                var graphQlRequest = new GraphQLRequest(@"query q($input: tokenExchange!) {
                                                            tokenExchange(input: $input){
                                                                   accessToken{
                                                                        hint
                                                                        authority
                                                                        expires_in
                                                                        access_token
                                                                        refresh_token
                                                                        token_type
                                                                        httpHeaders
                                                                        {
	                                                                        name
                                                                            value
                                                                        }
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

                var graphQLResponse = await graphQLHttpClient.PostAsync(graphQlRequest);
                graphQLResponse.ShouldNotBeNull();
                var bindResponse =
                    (List<TokenExchangeResponse>)graphQLResponse
                        .GetDataFieldAs<List<TokenExchangeResponse>>("tokenExchange"); //data->appIdentityCreate is casted as AppIdentityResponse
                bindResponse.ShouldNotBeNull();
                bindResponse.Count.ShouldBeGreaterThan(0);
                var handler = new JwtSecurityTokenHandler();
                var tokenS = handler.ReadToken(bindResponse[0].accessToken.access_token) as JwtSecurityToken;

                tokenS.ShouldNotBeNull();
                access_token = bindResponse[0].accessToken.access_token;


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

        [Fact]
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
        [Fact]
        public async Task App_identity_bind_appId_outofrange()
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
                            appId = Guid.NewGuid().ToString() + Guid.NewGuid().ToString(),
                            machineId = "machineId 001"
                        }
                    }
                };

                var response = graphQLHttpClient.PostAsync(appIdentityCreate).GetAwaiter().GetResult();
                response.Errors.ShouldNotBeNull();

            }
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
                            appId = "",
                            machineId = "machineId 001"
                        }
                    }
                };

                var response = graphQLHttpClient.PostAsync(appIdentityCreate).GetAwaiter().GetResult();
                response.Errors.ShouldNotBeNull();

            }

        }
        [Fact]
        public async Task App_identity_bind_machineId_outofrange()
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
                            machineId = Guid.NewGuid().ToString() + Guid.NewGuid().ToString(),
                            appId = Guid.NewGuid().ToString()
                        }
                    }
                };

                var response = graphQLHttpClient.PostAsync(appIdentityCreate).GetAwaiter().GetResult();
                response.Errors.ShouldNotBeNull();

            }
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
                            machineId = "",
                            appId = Guid.NewGuid().ToString()
                        }
                    }
                };

                var response = graphQLHttpClient.PostAsync(appIdentityCreate).GetAwaiter().GetResult();
                response.Errors.ShouldNotBeNull();

            }

        }
        [Fact]
        public async Task App_identity_bind_missing_unauthorized_subject()
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
                            machineId = "machineId 001",
                            subject = Guid.NewGuid().ToString()
                        }
                    }
                };
                var graphQLResponse = await graphQLHttpClient.PostAsync(appIdentityCreate);

                graphQLResponse.Errors.ShouldNotBeNull();

            }
        }

        async Task<ClientCredentialsResponse> FetchB2BAccessTokenAsync()
        {
            var client = _fixture.Client;

            var dict = new Dictionary<string, string>
            {
                {"grant_type", "client_credentials"},
                {"client_id", "b2b-client"},
                {"client_secret", "secret"}
            };



            var req = new HttpRequestMessage(HttpMethod.Post, "connect/token")
            {
                Content = new FormUrlEncodedContent(dict)
            };
            var response = await client.SendAsync(req);


            var jsonString = await response.Content.ReadAsStringAsync();

            var clientCredentialsResponse = JsonConvert.DeserializeObject<ClientCredentialsResponse>(jsonString);
            return clientCredentialsResponse;


        }
        [Fact]
        public async Task App_identity_bind_missing_authorized_subject()
        {
            var clientCredentialsResponse = await FetchB2BAccessTokenAsync();
            clientCredentialsResponse.ShouldNotBeNull();
            clientCredentialsResponse.access_token.ShouldNotBeNull();
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
                            machineId = "machineId 001",
                            subject = Guid.NewGuid().ToString()
                        }
                    }
                };
                graphQLHttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {clientCredentialsResponse.access_token}");
                graphQLHttpClient.DefaultRequestHeaders.Add("x-authScheme", $"self-testserver");
                var graphQLResponse = await graphQLHttpClient.PostAsync(appIdentityCreate);
                graphQLResponse.ShouldNotBeNull();
                var appIdentityResponse = graphQLResponse.GetDataFieldAs<AppIdentityResultModel>("appIdentityCreate"); //data->appIdentityCreate is casted as AppIdentityResponse
                appIdentityResponse.ShouldNotBeNull();
                var handler = new JwtSecurityTokenHandler();
                var tokenS = handler.ReadToken(appIdentityResponse.id_token) as JwtSecurityToken;

                tokenS.ShouldNotBeNull();
            }
        }
        [Fact]
        public async Task App_identity_bind_missing_authorized_subject_outofrange()
        {
            var clientCredentialsResponse = await FetchB2BAccessTokenAsync();
            clientCredentialsResponse.ShouldNotBeNull();
            clientCredentialsResponse.access_token.ShouldNotBeNull();
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
                            appId = Guid.NewGuid().ToString(),
                            machineId = Guid.NewGuid().ToString(),
                            subject = Guid.NewGuid().ToString() + Guid.NewGuid().ToString()
                        }
                    }
                };
                graphQLHttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {clientCredentialsResponse.access_token}");
                graphQLHttpClient.DefaultRequestHeaders.Add("x-authScheme", $"self-testserver");
                var graphQLResponse = await graphQLHttpClient.PostAsync(appIdentityCreate);
                graphQLResponse.Errors.ShouldNotBeNull();

            }
        }
    }
}
