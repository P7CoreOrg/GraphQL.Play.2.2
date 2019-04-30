using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TokenExchange.Contracts
{
    public class TokenExchangeHandlerRouter : IPipelineTokenExchangeHandlerRouter, ITokenExchangeHandlerRouter
    {
        private IEnumerable<Lazy<ITokenExchangeHandler>> _tokenExchangeHandlers;
        Dictionary<string, ITokenExchangeHandler> _mapTokenExchangeHandlers;

        private Dictionary<string, ITokenExchangeHandler> MapTokenExchangeHandlers
        {
            get
            {
                if (_mapTokenExchangeHandlers == null)
                {
                    _mapTokenExchangeHandlers = new Dictionary<string, ITokenExchangeHandler>();
                    foreach (var tokenExchangeHandler in _tokenExchangeHandlers)
                    {
                        _mapTokenExchangeHandlers.Add(tokenExchangeHandler.Value.Name, tokenExchangeHandler.Value);
                    }
                }

                return _mapTokenExchangeHandlers;
            }
        }

        private IEnumerable<Lazy<IPipelineTokenExchangeHandler>> _pipelineTokenExchangeHandlers;
        Dictionary<string, IPipelineTokenExchangeHandler> _mapPipelineTokenExchangeHandlers;

        private Dictionary<string, IPipelineTokenExchangeHandler> MapPipelineTokenExchangeHandlers
        {
            get
            {
                if (_mapPipelineTokenExchangeHandlers == null)
                {
                    _mapPipelineTokenExchangeHandlers = new Dictionary<string, IPipelineTokenExchangeHandler>();
                    foreach (var pipelineTokenExchangeHandler in _pipelineTokenExchangeHandlers)
                    {
                        _mapPipelineTokenExchangeHandlers.Add(pipelineTokenExchangeHandler.Value.Name,
                            pipelineTokenExchangeHandler.Value);
                    }
                }

                return _mapPipelineTokenExchangeHandlers;
            }
        }

        public TokenExchangeHandlerRouter(
            IEnumerable<Lazy<ITokenExchangeHandler>> tokenExchangeHandlers,
            IEnumerable<Lazy<IPipelineTokenExchangeHandler>> pipelineTokenExchangeHandlers
        )
        {
            _tokenExchangeHandlers = tokenExchangeHandlers;
            _pipelineTokenExchangeHandlers = pipelineTokenExchangeHandlers;
        }

        public Task<bool> TokenExchangeHandlerExistsAsync(string tokenScheme)
        {
            return Task.FromResult(MapTokenExchangeHandlers.ContainsKey(tokenScheme));
        }

        public async Task<List<TokenExchangeResponse>> ProcessExchangeAsync(
            string tokenScheme,
            TokenExchangeRequest tokenExchangeRequest)
        {
            if (MapTokenExchangeHandlers.ContainsKey(tokenScheme))
            {
                var response =
                    await MapTokenExchangeHandlers[tokenScheme]
                        .ProcessExchangeAsync(tokenExchangeRequest);
                return response;
            }

            throw new Exception($"{tokenScheme} is not mapped to an ITokenExchangeHandler");
        }

        public Task<bool> PipelineTokenExchangeHandlerExistsAsync(string tokenScheme)
        {
            return Task.FromResult(MapPipelineTokenExchangeHandlers.ContainsKey(tokenScheme));
        }

        public async Task<List<TokenExchangeResponse>> ProcessFinalPipelineExchangeAsync(
            string tokenScheme,
            TokenExchangeRequest tokenExchangeRequest,
            Dictionary<string, List<KeyValuePair<string, string>>> mapOpaqueKeyValuePairs)
        {
            if (MapPipelineTokenExchangeHandlers.ContainsKey(tokenScheme))
            {
                var response =
                    await MapPipelineTokenExchangeHandlers[tokenScheme]
                        .ProcessExchangeAsync(tokenExchangeRequest, mapOpaqueKeyValuePairs);
                return response;
            }

            throw new Exception($"{tokenScheme} is not mapped to an IPipelineTokenExchangeHandler");
        }
    }
}