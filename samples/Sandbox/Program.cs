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

ISpaceAgentProvider provider = client.ServiceProvider.GetRequiredService<ISpaceAgentProvider>();
ISpaceAgent agent = await provider.GetAsync();

SpaceTuple tuple = new(1, 2, 3);
SpaceTemplate template = new(1, null, 3, 4);

await agent.PeekAsync(template, x =>
{
    Console.WriteLine(x);
    return Task.CompletedTask;
});

await agent.WriteAsync(new(1, 2, 3, 4));

Console.WriteLine("\nPress any key to terminate...\n");
Console.ReadKey();

await host.StopAsync();