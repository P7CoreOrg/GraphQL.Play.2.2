namespace TokenExchange.Contracts
{
    public class TokenMintingResponse
    {
        //
        // Summary:
        //     Gets the access token.
        public string AccessToken { get; set; }
        //
        // Summary:
        //     Gets the identity token.
        public string IdentityToken { get; set; }
        //
        // Summary:
        //     Gets the type of the token.
        public string TokenType { get; set; }
        //
        // Summary:
        //     Gets the refresh token.
        public string RefreshToken { get; set; }
        //
        // Summary:
        //     Gets the error description.
        public string ErrorDescription { get; set; }
        //
        // Summary:
        //     Gets the expires in.
        public int ExpiresIn { get; set; }
        public bool IsError { get; set; }
        public string Error { get; set; }
        public string Scheme { get; set; }
    }
}