using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AppIdentity.Models;
using Castle.Core.Logging;
using FakeItEasy;
using GraphQL.Client;
using GraphQL.Common.Request;
using GraphQLPlay.Contracts;
using GraphQLPlay.IdentityModelExtras;
using GraphQLPlayTokenExchangeOnlyApp.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using P7Core.GraphQLCore.Validators;
using Shouldly;
using Xunit;

namespace XUnitServer_TokenExchange
{
    public class TokenExchangeTests : IClassFixture<MyTestServerFixture>
    {
        public string ClientId => "arbitrary-resource-owner-client";
        public string ClientSecret => "secret";
        private readonly MyTestServerFixture _fixture;
        private GraphQLClientOptions _graphQLClientOptions;
        UriBuilder BuildGraphQLEndpoint()
        {
            var endpoint = new UriBuilder(_fixture.Client.BaseAddress.Scheme,
                _fixture.Client.BaseAddress.Authority);
            endpoint.Path = "/api/v1/GraphQL";
            return endpoint;
        }
        public TokenExchangeTests(MyTestServerFixture fixture)
        {
            _fixture = fixture;
            var endpoint = BuildGraphQLEndpoint();

            _graphQLClientOptions = new GraphQL.Client.GraphQLClientOptions()
            {
                HttpMessageHandler = _fixture.MessageHandler,
                EndPoint = endpoint.Uri
            };
        }
        [Fact]
        public void AssureFixture()
        {
            _fixture.ShouldNotBeNull();
            var client = _fixture.Client;
            client.ShouldNotBeNull();
        }
        [Fact]
        public async Task GraphQLController_MakeObjectResult()
        {
            var objResult = GraphQLController.MakeObjectResult("Hello", HttpStatusCode.BadRequest);
            objResult.ShouldNotBeNull();

        }
        [Fact]
        public async Task IOAuth2ConfigurationStore_test()
        {
            var services = _fixture.TestServer.Host.Services;
            var theService = services.GetRequiredService<IOAuth2ConfigurationStore>();
            theService.ShouldNotBeNull();
            var all = await theService.GetWellknownAuthoritiesAsync();
            all.ShouldNotBeNull();
            all.Count.ShouldBeGreaterThan(0);
            var authority = await theService.GetWellknownAuthorityAsync(SimpleObjectCoverage.GuidString);
            authority.ShouldBeNull();
            authority = await theService.GetWellknownAuthorityAsync(
                all[0].Scheme);
            authority.ShouldNotBeNull();

        }
        [Fact]
        public async Task GraphQLController_test()
        {
            var fakeLogger = A.Fake<ILogger<GraphQLController>>();
            var fakeHttpContextAccessor = A.Fake<IHttpContextAccessor>();

            var pluginValidationRules = _fixture.TestServer.Host.Services.GetRequiredService<IEnumerable<IPluginValidationRule>>();
            var summaryLogger = _fixture.TestServer.Host.Services.GetRequiredService<IScopedSummaryLogger>();
            summaryLogger.Add(SimpleObjectCoverage.GuidString, SimpleObjectCoverage.GuidString);
            var controller = new GraphQLController(fakeHttpContextAccessor, fakeLogger, null,
                null, null, pluginValidationRules, summaryLogger);
            var result = await controller.ProcessQueryAsync(null);


        }
        [Fact]
        public async Task ServiceProvider_test()
        {
            var services = _fixture.TestServer.Host.Services;
            var discoverCacheContainerFactory = services.GetRequiredService<DiscoverCacheContainerFactory>();
            discoverCacheContainerFactory.ShouldNotBeNull();
            var all = discoverCacheContainerFactory.GetAll();
            all.ShouldNotBeNull();
            var noMas = discoverCacheContainerFactory.Get(SimpleObjectCoverage.GuidString);
            noMas.ShouldBeNull();
        }

        async Task<AppIdentityResultModel> FetchAppIdentityAsync()
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
                var appIdentityResponse =
                    graphQLResponse
                        .GetDataFieldAs<AppIdentityResultModel>(
                            "appIdentityCreate"); //data->appIdentityCreate is casted as AppIdentityResponse
                appIdentityResponse.ShouldNotBeNull();
                return appIdentityResponse;
            }
        }
        [Fact]
        public async Task success_app_identity_bind_and_exchange_selfvalidator()
        {
            string id_token = "";
            var appIdentityResponse = await FetchAppIdentityAsync();
            appIdentityResponse.ShouldNotBeNull();
            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadToken(appIdentityResponse.id_token) as JwtSecurityToken;

            tokenS.ShouldNotBeNull();
            id_token = appIdentityResponse.id_token;


            using (var graphQLHttpClient =
                new GraphQL.Client.GraphQLClient(_graphQLClientOptions))
            {
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

        }

        [Fact]
        public async Task success_unit_test_query()
        {
            using (var graphQLHttpClient =
                new GraphQL.Client.GraphQLClient(_graphQLClientOptions))
            {
                var graphQlRequest = new GraphQLRequest(
                    @"query{
                              unitTestCollection{
                                collection{
                                  age
                                  name
                                }
                              } 
                            }");

                var graphQLResponse = await graphQLHttpClient.PostAsync(graphQlRequest);
                graphQLResponse.ShouldNotBeNull();
                graphQLResponse.Errors.ShouldBeNull();
            }
        }
        [Fact]
        public async Task error_unit_test_query()
        {
            using (var graphQLHttpClient =
                new GraphQL.Client.GraphQLClient(_graphQLClientOptions))
            {
                var graphQlRequest = new GraphQLRequest(
                    @"query{
                              unitTestCollectionThrow{
                                collection{
                                  age
                                  name
                                }
                              } 
                            }");

                var graphQLResponse = await graphQLHttpClient.PostAsync(graphQlRequest);
                graphQLResponse.ShouldNotBeNull();
                graphQLResponse.Errors.ShouldNotBeNull();
            }
        }
        [Fact]
        public async Task success_app_identity_bind_and_exchange()
        {
            string id_token = "";
            var appIdentityResponse = await FetchAppIdentityAsync();
            appIdentityResponse.ShouldNotBeNull();
            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadToken(appIdentityResponse.id_token) as JwtSecurityToken;

            tokenS.ShouldNotBeNull();
            id_token = appIdentityResponse.id_token;


            using (var graphQLHttpClient =
                new GraphQL.Client.GraphQLClient(_graphQLClientOptions))
            {
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

        }
        [Fact]
        public async Task fail_Bad_token_exchange()
        {
            string id_token = Guid.NewGuid().ToString();



            using (var graphQLHttpClient =
                new GraphQL.Client.GraphQLClient(_graphQLClientOptions))
            {
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

        }
        class TokenScheme
        {
            public string token { get; set; }
            public string tokenScheme { get; set; }
        }
        [Fact]
        public async Task fail_missing_token_exchange()
        {
            string id_token = Guid.NewGuid().ToString();

            var tokens = new List<TokenScheme>();

            using (var graphQLHttpClient =
                new GraphQL.Client.GraphQLClient(_graphQLClientOptions))
            {
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
        }
        [Fact]
        public async Task fail_null_token_exchange()
        {
            string id_token = Guid.NewGuid().ToString();

            var tokens = new List<TokenScheme>();

            using (var graphQLHttpClient =
                new GraphQL.Client.GraphQLClient(_graphQLClientOptions))
            {
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
}
