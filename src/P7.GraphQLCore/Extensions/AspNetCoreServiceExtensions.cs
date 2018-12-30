using System;
using System.Collections.Generic;
using GraphQL;
using GraphQL.Execution;
using GraphQL.Http;
using GraphQL.Types;
using GraphQL.Validation;
using GraphQL.Validation.Complexity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using P7.GraphQLCore.Types;
using P7.GraphQLCore.Validators;

namespace P7.GraphQLCore.Extensions
{
    public static class AspNetCoreServiceExtensions
    {
        public static void AddGraphQLCoreTypes(this IServiceCollection services)
        {
            services.AddTransient<IQueryFieldRecordRegistrationStore,QueryFieldRecordRegistrationStore>();
            services.AddTransient<IMutationFieldRecordRegistrationStore, MutationFieldRecordRegistrationStore>();
            services.AddTransient<ISubscriptionFieldRecordRegistrationStore, SubscriptionFieldRecordRegistrationStore>();

            services.AddTransient<IDocumentBuilder, GraphQLDocumentBuilder>();
            services.AddTransient<IDocumentValidator, DocumentValidator>();
            services.AddTransient<IComplexityAnalyzer, ComplexityAnalyzer>();
            services.AddSingleton<IDocumentExecuter, DocumentExecuter>();

            services.TryAddTransient<IGraphQLJsonDocumentWriterOptions>(
                _ =>
                {
                    var graphQLJsonDocumentWriterOptions = new GraphQLJsonDocumentWriterOptions
                    {
                        Formatting = Formatting.None,
                        JsonSerializerSettings = new JsonSerializerSettings
                        {
                            ContractResolver = new CamelCasePropertyNamesContractResolver(),
                            DateFormatHandling = DateFormatHandling.IsoDateFormat,
                            DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'",
                            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                            Converters = new List<JsonConverter>()
                            {
                                new Newtonsoft.Json.Converters.IsoDateTimeConverter()
                            }
                        }
                    };
                    return graphQLJsonDocumentWriterOptions;
                });

            //services.AddSingleton<IDocumentWriter, GraphQLDocumentWriter>();
            services.AddSingleton<IDocumentWriter, DocumentWriter>();

            services.AddTransient<QueryCore>();
            services.AddTransient<MutationCore>();
            services.AddTransient<SubscriptionCore>();
            
            services.AddTransient<ISchema, SchemaCore>();
            services.TryAddTransient<Func<Type, GraphType>>(
                x =>
                {
                    var context = x.GetService<IServiceProvider>();
                    return t =>
                    {
                        var res = context.GetService(t);
                        return (GraphType)res;
                    };
                });

            services.AddSingleton<IPluginValidationRule, RequiresAuthValidationRule>();
            services.AddSingleton<IGraphQLAuthorizationCheck, OptOutGraphQLAuthorizationCheck>();
            services.AddSingleton<IGraphQLClaimsAuthorizationCheck, OptOutGraphQLClaimsAuthorizationCheck>();

            services.AddTransient<DynamicType>();

            services.AddSingleton<IDependencyResolver>(
                   c => new FuncDependencyResolver(type => c.GetRequiredService(type)));
        }
    }
}