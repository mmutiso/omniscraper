using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

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

        static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            services.AddHostedService<TweetListeningBackgroundService>();

            services.AddLogging();
            
        }

    }
}
