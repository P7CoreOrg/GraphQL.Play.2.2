namespace IdentityTokenExchange.GraphQL.Services
{
    public class TokenDescriptor
    {
        /// <summary>
        /// Any accepted token [id_token, etc] 
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// The type of token, i.e oidc,etc
        /// </summary>
        public string TokenScheme { get; set; }
        /// <summary>
        /// A wellknown lookup used to validate the token
        /// </summary>
        public string AuthorityKey { get; set; }
    }
}