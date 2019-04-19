using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using AppIdentity.Extensions;
using AuthRequiredDemoGraphQL.Extensions;
using B2BPublisher.Extensions;
using CustomerLoyaltyStore.Extensions;
using CustomerLoyalyStore.GraphQL.Extensions;
using DiscoveryHub.Extensions;
using GraphQLPlay.IdentityModelExtras;
using GraphQLPlay.IdentityModelExtras.Extensions;
using GraphQLPlay.Rollup.Extensions;
using IdentityModelExtras;
using IdentityModelExtras.Extensions;
using IdentityServer4.Configuration;
using IdentityServer4ExtensionGrants.Rollup.Extensions;
using IdentityServer4Extras.Extensions;
using IdentityServerRequestTracker.Extensions;
using Memstate.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MultiAuthority.AccessTokenValidation;
using Orders.Extensions;
using P7Core.BurnerGraphQL.Extensions;
using P7Core.BurnerGraphQL2.Extensions;
using P7Core.GraphQLCore.Extensions;
using P7Core.GraphQLCore.Stores;
using P7Core.ObjectContainers.Extensions;
using P7IdentityServer4.Extensions;
using Self.Validator.Extensions;
using Swashbuckle.AspNetCore.Swagger;
using TokenExchange.Contracts;
using TokenExchange.Contracts.Extensions;
using Utils.Extensions;
using static GraphQLPlay.Rollup.Extensions.AspNetCoreExtensions;
using static TokenExchange.Rollup.Extensions.AspNetCoreExtensions;

namespace IdentityServer4_Extension_Grants_App
{
    public class Startup :
        IExtensionGrantsRollupRegistrations,
        IGraphQLRollupRegistrations,
        ITokenExchangeRegistrations
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        public IConfiguration Configuration { get; }

        private ILogger<Startup> _logger;

        public Startup(IHostingEnvironment env, IConfiguration configuration, ILogger<Startup> logger)
        {
            _hostingEnvironment = env;
            Configuration = configuration;
            _logger = logger;
        }



        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {


            services.AddLogging();
            services.AddLazier();
            services.AddObjectContainer();  // use this vs a static to cache class data.
            services.AddOptions();
            services.AddDistributedMemoryCache();
            services.AddGraphQLPlayIdentityModelExtrasTypes();
            services.AddGraphQLPlayRollup(this);
            services.AddExtensionGrantsRollup(this);



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


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Daffy Duck",
                    policy => { policy.RequireClaim("client_namespace", "Daffy Duck"); });
            });

            var scheme = Configuration["authValidation:scheme"];

            var section = Configuration.GetSection("InMemoryOAuth2ConfigurationStore:oauth2");
            var oauth2Section = new Oauth2Section();
            section.Bind(oauth2Section);


            var query = from item in oauth2Section.Authorities
                        where item.Scheme == scheme
                        select item;
            var wellknownAuthority = query.FirstOrDefault();

            var authority = wellknownAuthority.Authority;
            List<SchemeRecord> schemeRecords = new List<SchemeRecord>()
            {  new SchemeRecord()
                {
                    Name = scheme,
                    JwtBearerOptions = options =>
                    {
                        options.Authority = authority;
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


            services.AddHttpContextAccessor();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();


            // Build the intermediate service provider then return it
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "IdentityServer4-Extensions-Grants-App", Version = "v1" });
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            return services.BuildServiceProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {


            app.UseLowercaseRewriter();

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

            app.UseIdentityServerRequestTrackerMiddleware();

            app.UseMvc();

            //MEMSTATE Journal stays in memory
            Config.Current.UseInMemoryFileSystem();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "IdentityServer4-Extensions-Grants-App V1");
            });

        }

        public void AddIdentityResources(IServiceCollection services, IIdentityServerBuilder builder)
        {
            _logger.LogInformation("AddIdentityResources to services");
            var identityResources = Configuration.LoadIdentityResourcesFromSettings();
            builder.AddInMemoryIdentityResources(identityResources);
        }

        public void AddClients(IServiceCollection services, IIdentityServerBuilder builder)
        {
            _logger.LogInformation("AddClients to services");
            var clients = Configuration.LoadClientsFromSettings();
            builder.AddInMemoryClientsExtra(clients);
        }

        public void AddApiResources(IServiceCollection services, IIdentityServerBuilder builder)
        {
            _logger.LogInformation("AddApiResources to services");
            var apiResources = Configuration.LoadApiResourcesFromSettings();
            builder.AddInMemoryApiResources(apiResources);
        }

        public void AddOperationalStore(IServiceCollection services, IIdentityServerBuilder builder)
        {
            _logger.LogInformation("AddOperationalStore to services");
            bool useRedis = Convert.ToBoolean(Configuration["appOptions:redis:useRedis"]);
            if (useRedis)
            {
                _logger.LogInformation("AddOperationalStore,Using Redis..");


                var redisConnectionString = Configuration["appOptions:redis:redisConnectionString"];
                _logger.LogInformation($"AddOperationalStore,redisConnectionString:{redisConnectionString.Substring(0, 70)}.....");
                builder.AddOperationalStore(options =>
                {
                    options.RedisConnectionString = redisConnectionString;
                    options.Db = 1;
                })
                .AddRedisCaching(options =>
                    {
                        options.RedisConnectionString = redisConnectionString;
                        options.KeyPrefix = "prefix";
                    });

                services.AddDistributedRedisCache(options =>
                {
                    options.Configuration = redisConnectionString;
                });
            }
            else
            {
                _logger.LogInformation("AddOperationalStore,Using AddInMemoryPersistedGrants..");
                builder.AddInMemoryPersistedGrants();
            }
        }

        public void AddSigningServices(IServiceCollection services, IIdentityServerBuilder builder)
        {
            _logger.LogInformation("AddSigningServices to services");

            bool useKeyVault = Convert.ToBoolean(Configuration["appOptions:keyVault:useKeyVault"]);
            bool useKeyVaultSigning = Convert.ToBoolean(Configuration["appOptions:keyVault:useKeyVaultSigning"]);

            _logger.LogInformation($"AddSigningServices:useKeyVault:{useKeyVault}");
            _logger.LogInformation($"AddSigningServices:useKeyVaultSigning:{useKeyVaultSigning}");

            if (useKeyVault)
            {
                builder.AddKeyVaultCredentialStore();
                services.AddKeyVaultTokenCreateServiceTypes();
                services.AddKeyVaultTokenCreateServiceConfiguration(Configuration);
                if (useKeyVaultSigning)
                {
                    // this signs the token using azure keyvault to do the actual signing
                    builder.AddKeyVaultTokenCreateService();
                }
            }
            else
            {
                _logger.LogInformation("AddSigningServices AddDeveloperSigningCredential");
                builder.AddDeveloperSigningCredential();
            }
        }

        public void AddGraphQLFieldAuthority(IServiceCollection services)
        {
            services.TryAddSingleton<IGraphQLFieldAuthority, InMemoryGraphQLFieldAuthority>();
            services.RegisterGraphQLCoreConfigurationServices(Configuration);
        }

        public void AddTokenValidators(IServiceCollection services)
        {
            services.AddInMemoryOAuth2ConfigurationStore();

            services.AddSelfTokenExchangeHandler();
            services.AddDemoTokenExchangeHandlers();

            services.AddSelfOIDCTokenValidator();
            var schemes = Configuration
                .GetSection("oidcSchemes")
                .Get<List<string>>();

            foreach (var scheme in schemes)
            {
                services.AddSingleton<ISchemeTokenValidator>(x =>
                {
                    var oidcTokenValidator = x.GetRequiredService<OIDCTokenValidator>();
                    oidcTokenValidator.TokenScheme = scheme;
                    return oidcTokenValidator;
                });

            }


        }

        public void AddGraphQLApis(IServiceCollection services)
        {
            // APIS
            services.AddTokenExchangeRollup(this);

            services.AddGraphQLCoreCustomLoyaltyTypes();
            services.AddGraphQLOrders();
            services.AddBurnerGraphQL();
            services.AddBurnerGraphQL2();
            services.AddGraphQLAppIdentityTypes();
            services.AddGraphQLDiscoveryTypes();
            services.AddInMemoryDiscoveryHubStore();
            services.AddB2BPublisherTypes();
            services.AddInMemoryB2BPlublisherStore();
            services.AddCustomerLoyalty();
            services.AddGraphQLAuthRequiredQuery();
        }

        public Action<IdentityServerOptions> GetIdentityServerOptions()
        {
            Action<IdentityServerOptions> identityServerOptions = options =>
            {
                options.InputLengthRestrictions.RefreshToken = 256;
                options.Caching.ClientStoreExpiration = TimeSpan.FromMinutes(5);
            };
            return identityServerOptions;
        }
    }
}
