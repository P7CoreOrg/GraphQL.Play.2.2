namespace TokenExchange.Contracts
{
    public class TokenDescriptor
    {
        /// <summary>
        /// Any accepted token [id_token, etc] 
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// A hint as to what this token is.  i.e.  id_token of an iot, or id_token of a user, etc
        /// This is a way route this token to a custom exchange
        /// </summary>
        public string TokenScheme { get; set; }
       
    }
}