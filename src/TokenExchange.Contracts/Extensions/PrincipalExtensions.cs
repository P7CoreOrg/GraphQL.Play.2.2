using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace TokenExchange.Contracts.Extensions
{
    public static class PrincipalExtensions
    {
        public static string GetSubjectFromPincipal(this ClaimsPrincipal principal)
        {
            var query = from item in principal.Claims
                        where item.Type == ClaimTypes.NameIdentifier || item.Type == "sub"
                        select item.Value;
            var subject = query.FirstOrDefault();
            return subject;
        }
    }
}
