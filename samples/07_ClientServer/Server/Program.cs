using OrleanSpaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using OrleanSpaces.Tuples;

var host = Host.CreateDefaultBuilder(args)
    .UseOrleans(siloBuilder =>
    {
        siloBuilder.UseLocalhostClustering();
        siloBuilder.AddOrleanSpaces();  // adding space services in the silo(s), means we can use the agents in the silo(s) too!
        siloBuilder.AddMemoryStreams(Constants.PubSubProvider);
        siloBuilder.AddMemoryGrainStorage(Constants.PubSubStore);
        siloBuilder.AddMemoryGrainStorage(Constants.StorageName);
    })
    .ConfigureLogging(builder => builder.AddConsole())
    .Build();

await host.StartAsync();

Console.WriteLine("Connected to the tuple space.\n\n");

ISpaceAgent agent = host.Services.GetRequiredService<ISpaceAgent>();

CancellationTokenSource cts = new();
cts.CancelAfter(10_000);

while (!cts.IsCancellationRequested)
{
    SpaceTuple serverTuple = new("SERVER");
    await agent.WriteAsync(serverTuple);
    Console.WriteLine($"WRITE: {serverTuple}");

    SpaceTuple clientTuple = await agent.PeekAsync(new("CLIENT"));
    if (clientTuple.Length > 0)
    {
        Console.WriteLine($"READ: {clientTuple}");
    }

    await Task.Delay(1000);
}

Console.WriteLine("\nPress any key to terminate...\n");
Console.ReadLine();

await host.StopAsync();
