using OrleanSpaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using OrleanSpaces.Tuples;
using OrleanSpaces.Tuples.Specialized;

var host = new HostBuilder()
    .UseOrleansClient(builder =>
    {
        builder.AddOrleanSpaces();
        builder.UseLocalhostClustering();
        builder.AddMemoryStreams(Constants.PubSubProvider);
    })
    .Build();

var client = host.Services.GetRequiredService<IClusterClient>();
await host.StartAsync();

Console.WriteLine("Connected to the tuple space.\n\n");

SpaceTuple tuple1 = new(1, 1, 1);
SpaceTuple tuple2 = new SpaceTuple(2, 2, 2);
var tuple3 = new SpaceTuple(3, 3, 3);

// test anything here...

Console.WriteLine("\nPress any key to terminate...\n");
Console.ReadKey();

await host.StopAsync();

class T
{
    public T()
    {
        SpaceTuple tuple1 = new(1, 1, 1);
        SpaceTuple tuple2 = new SpaceTuple(2, 2, 2);
        var tuple3 = new SpaceTuple(3, 3, 3);
    }
}