﻿using System;
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
            services.AddScoped<ILoadApplicationCredentials, EnvironmentVariablesKeysLoader>();

            TwitterKeys keys = services.BuildServiceProvider()
                    .GetRequiredService<ILoadApplicationCredentials>()
                    .Load();

            services.AddSingleton<TwitterKeys>(keys);
            services.AddScoped<OmniScraperContext>();
            services.AddScoped<ITwitterRepository, LinqToTwitterRepository>();
            services.AddScoped<TweetProcessingService>();
            services.Configure<TweetProcessorSettings>(context.Configuration.GetSection("TweetProcessorSettings"));
            services.AddDbContext<OmniscraperDbContext>((options) =>
            {
                options.UseSqlite(context.Configuration.GetConnectionString("Sqlite"));
            });
            services.AddScoped<IScraperRepository, ScraperRepository>();

            services.AddHostedService<TweetListeningBackgroundService>();
            services.AddLogging();    
        }

    }
}