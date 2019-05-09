using System.Net.Http;

namespace TestServerFixture
{
    public interface ITestServerFixture
    {
        HttpClient Client { get; }
        HttpMessageHandler MessageHandler { get; }
    }
}