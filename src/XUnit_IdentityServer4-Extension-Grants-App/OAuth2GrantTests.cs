using Newtonsoft.Json;
using Shouldly;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace XUnit_IdentityServer4_Extension_Grants_App
{
    public class OAuth2GrantTests : IClassFixture<MyTestServerFixture>
    {
        public class ClientCredentialsResponse
        {
            public string access_token { get; set; }
            public int expires_in { get; set; }
            public string token_type { get; set; }
           
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
    }

}
