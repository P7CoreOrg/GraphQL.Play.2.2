using System.Net.Http;

namespace GraphQLPlay.IdentityModelExtras
{
    public class NullDefaultHttpClientFactory : IDefaultHttpClientFactory
    {
        public HttpMessageHandler HttpMessageHandler { get; }
        public HttpClient HttpClient { get; }
    }
}