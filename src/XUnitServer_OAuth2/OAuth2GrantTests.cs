using Newtonsoft.Json;
using Shouldly;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using XUnitTestServerBase;

namespace XUnitServer_OAuth2
{
    public class OAuth2GrantTests : TestServerBaseTests
    {
        public class ClientCredentialsResponse
        {
            public string access_token { get; set; }
            public int expires_in { get; set; }
            public string token_type { get; set; }

        }
        public class ArbitraryResourceOwnerResponse
        {
            public string access_token { get; set; }
            public int expires_in { get; set; }
            public string token_type { get; set; }
            public string refresh_token { get; set; }

        }
        private readonly MyTestServerFixture _fixture;

        public OAuth2GrantTests(MyTestServerFixture fixture)
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
        public async Task fail_client_credentials()
        {
            var client = _fixture.Client;

            var dict = new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" },
                { "client_id", "b2b-client" },
                { "client_secret", "bad" }
            };



            var req = new HttpRequestMessage(HttpMethod.Post, "connect/token")
            {
                Content = new FormUrlEncodedContent(dict)
            };
            var response = await client.SendAsync(req);
            response.StatusCode.ShouldNotBe(System.Net.HttpStatusCode.OK);

        }
        [Fact]
        public async Task success_client_credentials()
        {
            var client = _fixture.Client;

            var dict = new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" },
                { "client_id", "b2b-client" },
                { "client_secret", "secret" }
            };



            var req = new HttpRequestMessage(HttpMethod.Post, "connect/token")
            {
                Content = new FormUrlEncodedContent(dict)
            };
            var response = await client.SendAsync(req);
            response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);

            var jsonString = await response.Content.ReadAsStringAsync();
            jsonString.ShouldNotBeNullOrWhiteSpace();
            var clientCredentialsResponse = JsonConvert.DeserializeObject<ClientCredentialsResponse>(jsonString);


            clientCredentialsResponse.ShouldNotBeNull();

            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadToken(clientCredentialsResponse.access_token) as JwtSecurityToken;

            tokenS.ShouldNotBeNull();
        }
        [Fact]
        public async Task success_arbitrary_resource_owner()
        {
            var client = _fixture.Client;

            var dict = new Dictionary<string, string>
            {
                { "grant_type", "arbitrary_resource_owner" },
                { "client_id", "arbitrary-resource-owner-client" },
                { "client_secret", "secret" },
                { "scope", "offline_access wizard" },
                { "arbitrary_claims", "{\"top\":[\"TopDog\"]}" },
                { "subject", "BugsBunny" },
                { "access_token_lifetime", "3600" },
                { "arbitrary_amrs", "[\"A\",\"D\",\"C\"]" },
                { "arbitrary_audiences", "[\"cat\",\"dog\"]" },
                { "custom_payload", "{\"some_string\": \"data\",\"some_number\": 1234,\"some_object\": { \"some_string\": \"data\",\"some_number\": 1234},\"some_array\": [{\"a\": \"b\"},{\"b\": \"c\"}]}" }
            };



            var req = new HttpRequestMessage(HttpMethod.Post, "connect/token")
            {
                Content = new FormUrlEncodedContent(dict)
            };
            var response = await client.SendAsync(req);
            response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);

            var jsonString = await response.Content.ReadAsStringAsync();
            jsonString.ShouldNotBeNullOrWhiteSpace();
            var arbitraryResourceOwnerResponse = JsonConvert.DeserializeObject<ArbitraryResourceOwnerResponse>(jsonString);


            arbitraryResourceOwnerResponse.ShouldNotBeNull();

            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadToken(arbitraryResourceOwnerResponse.access_token) as JwtSecurityToken;

            tokenS.ShouldNotBeNull();
        }
    }

}
