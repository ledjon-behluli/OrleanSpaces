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

_ = Task.Run(async () =>
{
    await foreach (var tuple in agent.EnumerateAsync())
    {
        Console.WriteLine(tuple);
    }
});

CancellationTokenSource cts = new();
cts.CancelAfter(10_000);

int i = 0;
while (!cts.IsCancellationRequested)
{
    SpaceTuple tuple = new(i);
    await agent.WriteAsync(tuple);
    await Task.Delay(1000);

    i++;
}

Console.WriteLine("\nPress any key to terminate...");
Console.ReadKey();

await host.StopAsync();