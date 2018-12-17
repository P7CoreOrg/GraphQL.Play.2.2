using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using CustomerLoyaltyStore.Extensions;
using CustomerLoyalyStore.GraphQL.Extensions;
using IdentityModelExtras;
using IdentityModelExtras.Extensions;
using IdentityTokenExchange.GraphQL.Extensions;
using Memstate.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using MultiAuthority.AccessTokenValidation;
using P7.Core.Cache;
using P7.GraphQLCore;
using P7.GraphQLCore.Extensions;
using P7.GraphQLCore.Stores;
using TheApp.Services;

namespace TheApp
{
    public class Startup
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            Configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddObjectCache();  // use this vs a static to cache class data.
            services.AddOptions();

            services.AddMemoryCache();
            services.AddIdentityModelExtrasTypes();
            services.AddSingleton<ConfiguredDiscoverCacheContainerFactory>();
            services.AddScoped<IJsonFileLoader, JsonFileLoader>();
            services.AddScoped<IRemoteJsonFileLoader, RemoteJsonFileLoader>();

            services.AddGraphQLCoreTypes();
            services.AddGraphQLCoreCustomLoyaltyTypes();
            services.AddGraphQLIdentityTokenExchangeTypes();
            services.TryAddSingleton<IGraphQLFieldAuthority, InMemoryGraphQLFieldAuthority>();
            services.RegisterGraphQLCoreConfigurationServices(Configuration);

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    corsBuilder => corsBuilder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });



            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddAuthorization(options =>
                {
                    options.AddPolicy("Daffy Duck",
                        policy => { policy.RequireClaim("client_namespace", "Daffy Duck"); });
                });

            List<SchemeRecord> schemeRecords = new List<SchemeRecord>()
            {  new SchemeRecord()
                {
                    Name = "One",
                    JwtBearerOptions = options =>
                    {
                        options.Authority = "https://p7identityserver4.azurewebsites.net";
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = false,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true
                        };
                        options.Events = new JwtBearerEvents
                        {
                            OnMessageReceived = context =>
                            {
                                return Task.CompletedTask;
                            },
                            OnTokenValidated = context =>
                            {

                                ClaimsIdentity identity = context.Principal.Identity as ClaimsIdentity;
                                if (identity != null)
                                {
                                    // Add the access_token as a claim, as we may actually need it
                                    var accessToken = context.SecurityToken as JwtSecurityToken;
                                    if (accessToken != null)
                                    {
                                        if (identity != null)
                                        {
                                            identity.AddClaim(new Claim("access_token", accessToken.RawData));
                                        }
                                    }
                                }

                                return Task.CompletedTask;
                            }
                        };
                    }

                },
            };
            
            services.AddAuthentication("Bearer")
                .AddMultiAuthorityAuthentication(schemeRecords);
            
            services.AddHttpContextAccessor(); services.AddHttpContextAccessor();
            services.AddCustomerLoyalty();
        
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc();
            //MEMSTATE Journal stays in memory
            Config.Current.UseInMemoryFileSystem();
        }
         
    }
}
