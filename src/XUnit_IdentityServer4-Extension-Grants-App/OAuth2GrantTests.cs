using Shouldly;
using System;
using Xunit;

namespace XUnit_IdentityServer4_Extension_Grants_App
{
    public class OAuth2GrantTests : IClassFixture<MyTestServerFixture>
    {
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
    }
     
}
