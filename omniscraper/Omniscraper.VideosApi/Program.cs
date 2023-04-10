using LinqToTwitter;
using Omniscraper.Core.Infrastructure;
using Omniscraper.VideosApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddLogging();

ILogger<EnvironmentVariablesKeysLoader> logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<EnvironmentVariablesKeysLoader>>();
ILoadApplicationKeys loader = new EnvironmentVariablesKeysLoader(logger);
builder.Services.AddSingleton(loader.LoadTwitterKeys());
builder.Services.AddSingleton<OmniScraperContext>();
builder.Services.AddSingleton<TwitterRepositoryFactory>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseAuthorization();

app.MapControllers();

app.Run();
