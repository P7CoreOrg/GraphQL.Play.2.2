
using GraphQLPlay.IdentityModelExtras;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;

namespace XUnitHelpers
{
    public class TestDefaultHttpClientFactory : IDefaultHttpClientFactory
    {
        public static TestServer TestServer { get; set; }
        public HttpMessageHandler HttpMessageHandler => TestServer.CreateHandler();
        public HttpClient HttpClient => TestServer.CreateClient();
    }
}
