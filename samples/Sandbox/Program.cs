using OrleanSpaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using OrleanSpaces.Tuples;

var host = new HostBuilder()
    .UseOrleansClient(builder =>
    {
        builder.AddOrleanSpaces(options => options.EnabledSpaces = SpaceKind.Generic | SpaceKind.Int);
        builder.UseLocalhostClustering();
        builder.AddMemoryStreams(Constants.PubSubProvider);
    })
    .Build();

var client = host.Services.GetRequiredService<IClusterClient>();
await host.StartAsync();

Console.WriteLine("Connected to the tuple space.\n\n");

ISpaceTuple spaceTuple = null
// test anything here...

Console.WriteLine("\nPress any key to terminate...\n");
Console.ReadKey();

await host.StopAsync();