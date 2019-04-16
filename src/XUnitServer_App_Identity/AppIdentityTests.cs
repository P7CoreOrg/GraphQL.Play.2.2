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
using GraphQL.Client;
using IdentityTokenExchangeGraphQL.Models;
using Xunit;
using XUnitTestServerBase;
using TokenExchange.Contracts;

namespace XUnitServer_App_Identity
{
    public class AppIdentityTests : TestServerBaseTests
    {

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
        [Fact]
        public void AssureFixture()
        {
            _fixture.ShouldNotBeNull();
            var client = _fixture.Client;
            client.ShouldNotBeNull();
        }
        UriBuilder BuildGraphQLEndpoint()
        {
            var endpoint = new UriBuilder(_fixture.Client.BaseAddress.Scheme,
              _fixture.Client.BaseAddress.Authority);
            endpoint.Path = "/api/v1/GraphQL";
            return endpoint;
        }
        [Fact]
        public async Task success_app_identity_bind()
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
    }
}
