using Orleans;
using OrleanSpaces;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

await Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(config => config.AddJsonFile("appsettings.json"))
    .ConfigureServices(services =>
    {
        services.AddTupleSpace();
        services.AddHostedService<Worker>();
    })
    .ConfigureLogging((context, builder) =>
    {
        builder.ClearProviders();
        builder.AddConfiguration(context.Configuration.GetSection("Logging"));
        builder.AddConsole();
    })
    .RunConsoleAsync();


public static class ExchangeKeys
{
    public const string PASSWORD_SEARCH = "PASSWORD_SEARCH";
    public const string PASSWORD_FOUND = "PASSWORD_FOUND";
    public const string TERMINATE = "TERMINATE";
}