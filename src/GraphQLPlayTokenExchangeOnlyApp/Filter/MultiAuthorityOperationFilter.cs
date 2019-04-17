using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace GraphQLPlayTokenExchangeOnlyApp.Filter
{

    public class MultiAuthorityOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<IParameter>();

            var authPresent = context.MethodInfo.GetCustomAttributes(typeof(AuthorizeAttribute), false).Any();
            if (authPresent)
            {
                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = "Authorization",
                    In = "header",
                    Type = "string",
                    Required = true // set to false if this is optional
                });


                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = "x-authScheme",
                    In = "header",
                    Type = "string",
                    Required = true // set to false if this is optional
                });
            }
        }
    }
}
