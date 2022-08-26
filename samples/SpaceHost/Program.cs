using Orleans.Hosting;
using Microsoft.Extensions.Logging;
using OrleanSpaces;
using Microsoft.Extensions.Hosting;
using Orleans;

var host = new SiloHostBuilder()
    .UseLocalhostClustering()
    .UseTupleSpace()
        .AddSimpleMessageStreamProvider(StreamNames.PubSubProvider)
        .AddMemoryGrainStorage(StreamNames.PubSubStore)
        .AddMemoryGrainStorage(StorageNames.TupleSpaceStore)
    .ConfigureLogging(builder => builder.AddConsole())
    .Build();

await host.StartAsync();

Console.WriteLine("\n\nPress any key to terminate...\n\n");
Console.ReadLine();

await host.StopAsync();