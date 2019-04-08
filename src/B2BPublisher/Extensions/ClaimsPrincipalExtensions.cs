using B2BPublisher.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace B2BPublisher.Extensions
{
    public static class AuthContextExtensions
    {
        public static (string clientId, string clientNamespace, List<string> scopes) ToClientInfoFromPincipal(this ClaimsPrincipal principal )
        {
            var query = from item in principal.Claims
                        where item.Type == "client_id"
                        select item.Value;
            var clientId = query.FirstOrDefault();

            query = from item in principal.Claims
                    where item.Type == "client_namespace"
                    select item.Value;
            var clientNamespace = query.FirstOrDefault();

            query = from item in principal.Claims
                    where item.Type == "scope"
                    select item.Value;
            var scopes = query.ToList();
            return (clientId, clientNamespace, scopes);

        }
        public static AuthContext ToAuthContext(this ClaimsPrincipal principal)
        {
            var clientInfo = principal.ToClientInfoFromPincipal();
            return new AuthContext
            {
                ClientId = clientInfo.clientId,
                ClientNamespace = clientInfo.clientNamespace,
                Scopes = clientInfo.scopes

            };
        }
        
    }

}
