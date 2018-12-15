using System;

namespace IdentityTokenExchange.GraphQL.Models
{
    public class BindInputModel : IComparable
    {
        /// <summary>
        /// Any accepted token [id_token, etc] 
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// The type of token, i.e oidc
        /// </summary>
        public string TokenScheme { get; set; }
        /// <summary>
        /// A wellknown lookup used to validate the token
        /// </summary>
        public string AuthorityKey { get; set; }
        public int CompareTo(object obj)
        {
            if (Equals(obj))
                return 0;
            return -1;
        }
        public override bool Equals(object obj)
        {
            return ShallowEquals(obj);
        }
        public bool ShallowEquals(object obj)
        {
            var other = obj as BindInputModel;
            if (other == null)
            {
                return false;
            }
            if (other.Token != this.Token ||
                other.TokenScheme != this.TokenScheme ||
                other.AuthorityKey != this.AuthorityKey  
            )
            {
                return false;
            }
            return true;
        }
    }
}