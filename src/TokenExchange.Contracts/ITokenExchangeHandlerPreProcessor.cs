using System;
using System.Text;
using System.Threading.Tasks;

namespace TokenExchange.Contracts
{
    public interface ITokenExchangeHandlerPreProcessor
    {
        string Name { get; }
        Task ProcessAsync(ref TokenExchangeRequest tokenExchangeRequest);
    }
}
