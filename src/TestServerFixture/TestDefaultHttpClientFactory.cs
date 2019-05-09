﻿using System.Net.Http;
using GraphQLPlay.IdentityModelExtras;
using Microsoft.AspNetCore.TestHost;

namespace TestServerFixture
{
    public class TestDefaultHttpClientFactory : IDefaultHttpClientFactory
    {
        public static TestServer TestServer { get; set; }
        public HttpMessageHandler HttpMessageHandler => TestServer.CreateHandler();
        public HttpClient HttpClient => TestServer.CreateClient();
    }
}