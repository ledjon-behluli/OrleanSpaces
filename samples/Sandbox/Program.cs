using OrleanSpaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using OrleanSpaces.Tuples;

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

await agent.WriteAsync(new SpaceTuple(1, 2, 3));
await agent.ClearAsync();

Console.WriteLine("\nPress any key to terminate...\n");
Console.ReadKey();

await host.StopAsync();