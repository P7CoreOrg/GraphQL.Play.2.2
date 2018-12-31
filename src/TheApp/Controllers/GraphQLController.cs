using System.Collections.Generic;
using GraphQL;
using GraphQL.Http;
using GraphQL.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using P7Core.GraphQLCore.Controllers;
using P7Core.GraphQLCore.Validators;

namespace TheApp.Controllers
{
    [Route("api/v1/[controller]")]
    public class GraphQLController : GraphQLControllerBase<GraphQLController>
    {
        public GraphQLController(IHttpContextAccessor httpContextAccessor, ILogger<GraphQLController> logger, IDocumentExecuter executer, IDocumentWriter writer, ISchema schema, IEnumerable<IPluginValidationRule> pluginValidationRules) : base(httpContextAccessor, logger, executer, writer, schema, pluginValidationRules)
        {
        }
    }
}