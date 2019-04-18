using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TokenExchange.Contracts;
using TokenExchange.Contracts.Models;
using Utils.Models;

namespace BriarRabbitTokenExchange
{
    [Route("api/token_exchange")]
    [ApiController]
    public class BriarRabbitController : ControllerBase
    {
        private ILogger<BriarRabbitController> _logger;


        public BriarRabbitController(
            ILogger<BriarRabbitController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Authorize]
        [Route("briar_rabbit")]
        public async Task<List<TokenExchangeResponse>> PostProcessTokenExchangeAsync(TokenExchangeRequest tokenExchangeRequest)
        {
            return new List<TokenExchangeResponse>()
            {
                new TokenExchangeResponse()
                {
                    access_token = $"briar_rabbit_access_token_{Guid.NewGuid().ToString()}",
                    refresh_token = $"briar_rabbit_refresh_token_{Guid.NewGuid().ToString()}",
                    expires_in = 1234,
                    token_type = $"briar_rabbit_Type",
                    authority =
                        $"https://briar.rabbit.com/authority",
                    HttpHeaders = new List<HttpHeader>
                    {
                        new HttpHeader() {Name = "x-authScheme", Value = "rabbit"}
                    }

                }
            };
        }
    }
}
