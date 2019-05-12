using System.Net.Http;
using GraphQLPlay.IdentityModelExtras;

namespace GraphQLPlayApiOnlyApp
{
    public class DefaultHttpClientFactory : IDefaultHttpClientFactory
    {
        public HttpMessageHandler HttpMessageHandler { get; set; }
        public HttpClient HttpClient { get { return new HttpClient(); } }
    }
}