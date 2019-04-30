using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TokenExchange.Contracts
{
    public class TokenExchangeHandlerRouter : IPipelineTokenExchangeHandlerRouter, ITokenExchangeHandlerRouter
    {
        private IEnumerable<ITokenExchangeHandler> _tokenExchangeHandlers;
        private IEnumerable<IPipelineTokenExchangeHandler> _pipelineTokenExchangeHandlers;
        readonly Dictionary<string, ITokenExchangeHandler> _mapTokenExchangeHandlers;
        readonly Dictionary<string, IPipelineTokenExchangeHandler> _mapPipelineTokenExchangeHandlers;

        public TokenExchangeHandlerRouter(
            IEnumerable<ITokenExchangeHandler> tokenExchangeHandlers,
            IEnumerable<IPipelineTokenExchangeHandler> pipelineTokenExchangeHandlers
            )
        {
            _mapTokenExchangeHandlers = new Dictionary<string, ITokenExchangeHandler>();
            _mapPipelineTokenExchangeHandlers = new Dictionary<string, IPipelineTokenExchangeHandler>();
            _tokenExchangeHandlers = tokenExchangeHandlers;
            _pipelineTokenExchangeHandlers = pipelineTokenExchangeHandlers;
            foreach (var tokenExchangeHandler in _tokenExchangeHandlers)
            {
                _mapTokenExchangeHandlers.Add(tokenExchangeHandler.Name, tokenExchangeHandler);
            }
            foreach (var pipelineTokenExchangeHandler in _pipelineTokenExchangeHandlers)
            {
                _mapPipelineTokenExchangeHandlers.Add(pipelineTokenExchangeHandler.Name, pipelineTokenExchangeHandler);
            }
        }

        public async Task<List<TokenExchangeResponse>> ProcessExchangeAsync(
            string tokenScheme,
            TokenExchangeRequest tokenExchangeRequest)
        {
            if (_mapTokenExchangeHandlers.ContainsKey(tokenScheme))
            {
                var response =
                    await _mapTokenExchangeHandlers[tokenScheme]
                        .ProcessExchangeAsync(tokenExchangeRequest);
                return response;
            }
            throw new Exception($"{tokenScheme} is not mapped to an ITokenExchangeHandler");
        }

        public async Task<List<TokenExchangeResponse>> ProcessFinalPipelineExchangeAsync(
            string tokenScheme,
            TokenExchangeRequest tokenExchangeRequest,
            Dictionary<string, List<KeyValuePair<string, string>>> mapOpaqueKeyValuePairs)
        {
            if (_mapPipelineTokenExchangeHandlers.ContainsKey(tokenScheme))
            {
                var response =
                    await _mapPipelineTokenExchangeHandlers[tokenScheme]
                        .ProcessExchangeAsync(tokenExchangeRequest, mapOpaqueKeyValuePairs);
                return response;
            }
            throw new Exception($"{tokenScheme} is not mapped to an IPipelineTokenExchangeHandler");
        }

        public Task<bool> PipelineTokenExchangeHandlerExistsAsync(string tokenScheme)
        {
            return Task.FromResult(_mapPipelineTokenExchangeHandlers.ContainsKey(tokenScheme));
        }

        public Task<bool> TokenExchangeHandleExistsAsync(string tokenScheme)
        {
            return Task.FromResult(_mapTokenExchangeHandlers.ContainsKey(tokenScheme));
        }
    }
}