namespace IdentityTokenExchangeGraphQL.Models
{
    public class IdentityTokenModel
    {
        /// <summary>
        /// Any accepted token [id_token, etc] 
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// The type of token, i.e oidc
        /// </summary>
        public string TokenScheme { get; set; }
    }
}