using OrleanSpaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .UseOrleans(siloBuilder =>
    {
        siloBuilder.UseLocalhostClustering();
        siloBuilder.AddMemoryStreams(Constants.PubSubProvider);
        siloBuilder.AddMemoryGrainStorage(Constants.PubSubStore);
        siloBuilder.AddMemoryGrainStorage(Constants.StorageName);
    })
    .ConfigureLogging(builder => builder.AddConsole())
    .Build();

await host.StartAsync();

Console.WriteLine("\nPress any key to terminate...\n");
Console.ReadLine();

await host.StopAsync();
