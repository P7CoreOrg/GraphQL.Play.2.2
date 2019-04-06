using System;
using System.Collections.Generic;
using System.Text;
using B2BPublisher.Models;
using B2BPublisher.Mutation;
using Microsoft.Extensions.DependencyInjection;
using P7Core.GraphQLCore;

namespace B2BPublisher.Extensions
{
    public static class AspNetCoreServiceExtensions
    {
        public static void AddB2BPublisherTypes(this IServiceCollection services)
        {
            services.AddTransient<PublishStatusEnumType>();
            services.AddTransient<PublishStateInputType>();
            services.AddTransient<PublishStateResultType>();
            services.AddTransient<IMutationFieldRegistration, PublishStateMutation>();



        }
    }
}
