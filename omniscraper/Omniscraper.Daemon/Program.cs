using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

using Omniscraper.Core;
using Omniscraper.Core.Infrastructure;
using Omniscraper.Core.Storage;
using Omniscraper.Core.TwitterScraper;
using Microsoft.EntityFrameworkCore;
using Omniscraper.Core.TwitterScraper.ContentHandlers;
using Azure.Security.KeyVault.Secrets;
using Azure.Core;
using Azure.Identity;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DependencyCollector;

namespace Omniscraper.Daemon
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddJsonFile("appsettings.json", false);
            })
            .ConfigureLogging((context, builder) =>
            {
                builder.AddConsole((log) =>
                {

                });

                Log.Logger = new LoggerConfiguration()
                .Enrich.WithProperty("process", "omniscraper.daemon")
                .WriteTo
                .File("log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

                builder.AddSerilog(Log.Logger);

            }).ConfigureServices(ConfigureServices);

        static string GetEnvironmentKey(string key)
        {
            string envValue = Environment.GetEnvironmentVariable(key);
            if (string.IsNullOrEmpty(envValue))
                Console.WriteLine($"No value found for Variable: {key} ");

            return envValue;
        }

        static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            services.AddLogging();
            services.AddApplicationInsightsTelemetryWorkerService();
            services.ConfigureTelemetryModule<DependencyTrackingTelemetryModule>((module, o) => { module.EnableSqlCommandTextInstrumentation = true; });
            services.Configure<TweetProcessorSettings>(context.Configuration.GetSection("TweetProcessorSettings"));

            SecretClientOptions secretClientOptions = new SecretClientOptions
            {
                Retry =
                {
                    Delay = TimeSpan.FromSeconds(2),
                    MaxDelay = TimeSpan.FromSeconds(16),
                    MaxRetries = 5,
                    Mode = RetryMode.Exponential
                }
            };
            services.AddSingleton<SecretClient>(options => {

                string keyVaultUrlName = context.Configuration["KeyVault:UrlEnvironmentVariableName"];
                string keyVaultUrl = GetEnvironmentKey(keyVaultUrlName);
                return new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential(), secretClientOptions);
            });

            services.AddSingleton<ILoadApplicationKeys, AzureKeyVaultKeysLoader>();

            var kvClient = services.BuildServiceProvider()
                    .GetRequiredService<ILoadApplicationKeys>();
            TwitterKeys keys = kvClient
                    .LoadTwitterKeys();

            string videosApiBaseUrl = kvClient.LoadByKeyName(context.Configuration["TweetProcessorSettings:VideoApiBaseUrlKeyVaultName"]);

            
            services.AddHttpClient(context.Configuration["TweetProcessorSettings:VideoApiHttpClientName"], opt =>
            {
                opt.BaseAddress = new Uri(videosApiBaseUrl);
            });

            services.AddSingleton<VideosApiWrapper>();
            services.AddSingleton(keys);
            services.AddSingleton<OmniScraperContext>();
            services.AddSingleton<ITwitterRepository, LinqToTwitterRepository>();
            services.AddSingleton<TweetProcessingService>();
            
            services.AddDbContextFactory<OmniscraperDbContext>(options =>
            {
                string connectionStringKeyName = "dev-postres-connection-string";
                var logger  = services.BuildServiceProvider().GetRequiredService<ILogger<Program>>();
                logger.LogInformation($"Hosting environment: {context.HostingEnvironment.EnvironmentName}");
                if (!context.HostingEnvironment.IsDevelopment())
                {
                    connectionStringKeyName = "prod-postres-connection-string";
                }
                string connectionString  = services.BuildServiceProvider()
                            .GetRequiredService<ILoadApplicationKeys>()
                            .LoadByKeyName(connectionStringKeyName);

                var serverVersion = new MySqlServerVersion(new Version(5, 7));
                options.UseMySql(connectionString, serverVersion);
                options.UseSnakeCaseNamingConvention();
            });

            services.AddSingleton<IScraperRepository, ScraperRepository>();

            services.AddHostedService<TweetListeningBackgroundService>();
            services.AddSingleton<TwitterContentHandlerFactory>();
        }
    }
}
