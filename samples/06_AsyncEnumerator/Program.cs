using OrleanSpaces;
using OrleanSpaces.Tuples;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

var host = new HostBuilder()
    .ConfigureAppConfiguration(config => config.AddJsonFile("appsettings.json"))
    .UseOrleansClient(builder =>
    {
        builder.AddOrleanSpaces();
        builder.UseLocalhostClustering();
        builder.AddMemoryStreams(Constants.PubSubProvider);
    })
    .ConfigureLogging((context, builder) =>
    {
        builder.ClearProviders();
        builder.AddConfiguration(context.Configuration.GetSection("Logging"));
        builder.AddConsole();
    })
    .Build();

await host.StartAsync();

Console.WriteLine("Connected to the tuple space.\n\n");

ISpaceAgent agent = host.Services.GetRequiredService<ISpaceAgent>();

// TODO: Continue

Console.WriteLine("\nPress any key to terminate...");
Console.ReadKey();

await host.StopAsync();