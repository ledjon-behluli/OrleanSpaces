using Orleans.Hosting;
using Microsoft.Extensions.Logging;
using OrleanSpaces.Hosts;

var host = new SiloHostBuilder()
    .UseLocalhostClustering()
    .ConfigureTupleSpace()
    .ConfigureLogging(builder => builder.AddConsole())
    .AddMemoryGrainStorageAsDefault()
    .Build();

await host.StartAsync();

Console.WriteLine("\n\nPress any key to terminate...\n\n");
Console.ReadLine();

await host.StopAsync();
