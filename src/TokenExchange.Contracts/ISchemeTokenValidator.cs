namespace TokenExchange.Contracts
{
    public interface ISchemeTokenValidator: ITokenValidator
    {
        string TokenScheme { get; }
    }
}