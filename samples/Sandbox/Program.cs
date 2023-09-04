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

var agent = host.Services.GetRequiredService<ISpaceAgent>();

object[] objects = new object[] { 1, 2, 3 };
object[] values = new object[] { objects };

SpaceTuple tuple = new(values);

await agent.WriteAsync(new SpaceTuple(1, 2, 3));
await agent.WriteAsync(new SpaceTuple(1, 2, 3));
await agent.WriteAsync(new SpaceTuple(1, 2, 3));

await agent.ClearAsync();

Console.WriteLine("\nPress any key to terminate...\n");
Console.ReadKey();

await host.StopAsync();