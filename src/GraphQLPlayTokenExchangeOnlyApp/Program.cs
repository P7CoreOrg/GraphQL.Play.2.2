using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GraphQLPlayTokenExchangeOnlyApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();
            var logger = host.Services.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("Seeded the database.");
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var environmentName = hostingContext.HostingEnvironment.EnvironmentName;
                    LoadConfigurations(config, environmentName);
                    config.AddEnvironmentVariables();
                    config.AddUserSecrets<Startup>();
                })
                .UseStartup<Startup>()
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.ClearProviders();
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                    logging.AddDebug();
                    logging.AddEventSourceLogger();
                    logging.AddAzureWebAppDiagnostics();
                });

        public static void LoadConfigurations(IConfigurationBuilder config, string environmentName)
        {
            config
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile("appsettings.redis.json", optional: false, reloadOnChange: true)
            .AddJsonFile("appsettings.cosmos.json", optional: false, reloadOnChange: true)
            .AddJsonFile("appsettings.keyVault.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
            .AddJsonFile($"appsettings.{environmentName}.IdentityResources.json", optional: true)
            .AddJsonFile($"appsettings.{environmentName}.ApiResources.json", optional: true)
            .AddJsonFile($"appsettings.{environmentName}.Clients.json", optional: true)
            .AddJsonFile($"appsettings.{environmentName}.TokenExchange.json", optional: true)
            .AddJsonFile($"appsettings.graphql.json", optional: false, reloadOnChange: true);
         
        }
    }
}
