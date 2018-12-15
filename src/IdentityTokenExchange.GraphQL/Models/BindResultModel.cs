using System;

namespace IdentityTokenExchange.GraphQL.Models
{
    public class BindResultModel : IComparable
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string token_type { get; set; }
        public string refresh_token { get; set; }
        public string authority { get; set; }
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
            var other = obj as BindResultModel;
            if (other == null)
            {
                return false;
            }
            if (other.access_token != this.access_token ||
                other.expires_in != this.expires_in ||
                other.token_type != this.token_type ||
                other.refresh_token != this.refresh_token ||
                other.authority != this.authority
            )
            {
                return false;
            }
            return true;
        }
    }
}