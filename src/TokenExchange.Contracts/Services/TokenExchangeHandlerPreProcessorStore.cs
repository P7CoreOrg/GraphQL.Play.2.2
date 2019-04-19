using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TokenExchange.Contracts.Services
{
    public class TokenExchangeHandlerPreProcessorStore : ITokenExchangeHandlerPreProcessorStore
    {
        private IEnumerable<ITokenExchangeHandlerPreProcessor> _tokenExchangeHandlerPreProcessors;
        private Dictionary<string, ITokenExchangeHandlerPreProcessor> _dict;
        public TokenExchangeHandlerPreProcessorStore(
            IEnumerable<ITokenExchangeHandlerPreProcessor> tokenExchangeHandlerPreProcessors)
        {
            _tokenExchangeHandlerPreProcessors = tokenExchangeHandlerPreProcessors;

            _dict = new Dictionary<string, ITokenExchangeHandlerPreProcessor>();
            foreach (var tokenExchangeHandlerPreProcessor in _tokenExchangeHandlerPreProcessors)
            {
                _dict.TryAdd(tokenExchangeHandlerPreProcessor.Name, tokenExchangeHandlerPreProcessor);
            }
        }


        public List<ITokenExchangeHandlerPreProcessor> GetAll()
        {
            return _tokenExchangeHandlerPreProcessors.ToList();
        }

        public ITokenExchangeHandlerPreProcessor Get(string name)
        {
            ITokenExchangeHandlerPreProcessor tokenExchangeHandlerPreProcessor = null;
            _dict.TryGetValue(name, out tokenExchangeHandlerPreProcessor);
            return tokenExchangeHandlerPreProcessor;
        }
    }
}
