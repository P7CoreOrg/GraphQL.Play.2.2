﻿using GraphQL.Common.Request;
using Newtonsoft.Json;
using Shouldly;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using XUnitTestServerBase;

namespace XUnitServer_App_Identity
{
    public class AppIdentityTests : TestServerBaseTests
    {


        public class AppIdentityResponse
        {
            public string id_token { get; set; }
            public int expires_in { get; set; }
            public string authority { get; set; }


        }
        private readonly MyTestServerFixture _fixture;

        public AppIdentityTests(MyTestServerFixture fixture)
        {
            _fixture = fixture;
        }
        [Fact]
        public void AssureFixture()
        {
            _fixture.ShouldNotBeNull();
            var client = _fixture.Client;
            client.ShouldNotBeNull();
        }
        [Fact]
        public async Task success_app_identity_bind()
        {

            var endpoint = new UriBuilder(_fixture.Client.BaseAddress.Scheme,
                _fixture.Client.BaseAddress.Authority);
            endpoint.Path = "/api/v1/GraphQL";

            var graphQLClientOptions = new GraphQL.Client.GraphQLClientOptions()
            {
                HttpMessageHandler = _fixture.MessageHandler,
                EndPoint = endpoint.Uri
            };

            using (var graphQLHttpClient =
                new GraphQL.Client.GraphQLClient(graphQLClientOptions))
            {
                var appIdentityBind = new GraphQLRequest(@"query q($input: appIdentityBind!) {
                          appIdentityBind(input: $input){
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

                var graphQLResponse = await graphQLHttpClient.PostAsync(appIdentityBind);
                graphQLResponse.ShouldNotBeNull();
                var appIdentityResponse = graphQLResponse.GetDataFieldAs<AppIdentityResponse>("appIdentityBind"); //data->appIdentityBind is casted as AppIdentityResponse
                appIdentityResponse.ShouldNotBeNull();
                var handler = new JwtSecurityTokenHandler();
                var tokenS = handler.ReadToken(appIdentityResponse.id_token) as JwtSecurityToken;

                tokenS.ShouldNotBeNull();
            }
        }

    }
}