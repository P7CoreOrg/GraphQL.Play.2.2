using System.Collections.Generic;

namespace TokenExchange.Contracts
{
    public interface ITokenExchangeHandlerPreProcessorStore
    {
        List<ITokenExchangeHandlerPreProcessor> GetAll();
        ITokenExchangeHandlerPreProcessor Get(string name);
       
    }
}