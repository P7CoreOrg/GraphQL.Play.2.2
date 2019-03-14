using System.Threading.Tasks;

namespace TokenExchange.Contracts
{
    public interface ITokenMintingService
    {
        Task<TokenMintingResponse> MintResourceOwnerTokenAsync(ResourceOwnerTokenRequest resourceOwnerTokenRequest);
    }
}