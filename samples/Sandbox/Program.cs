using OrleanSpaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using OrleanSpaces.Tuples;
using OrleanSpaces.Tuples.Typed;

var host = new HostBuilder()
    .UseOrleansClient(builder =>
    {
        builder.AddOrleanSpaces(options =>
        {
            options.SpaceTuplesEnabled = true;
            options.IntTuplesEnabled = true;
        });
        builder.UseLocalhostClustering();
        builder.AddMemoryStreams(Constants.PubSubProvider);
    })
    .Build();

var client = host.Services.GetRequiredService<IClusterClient>();
await host.StartAsync();

Console.WriteLine("Connected to the tuple space.\n\n");


// SpaceTuple
var provider1 = client.ServiceProvider.GetRequiredService<ISpaceAgentProvider>();
var agent1 = await provider1.GetAsync();

SpaceTuple s_tuple1 = new(1, "2", 3);
SpaceTemplate s_template1 = new(1, null, 3);
SpaceTuple s_tuple2 = new(1, "2", 3, 4);
SpaceTemplate s_template2 = new(1, null, 3, null);

await agent1.WriteAsync(s_tuple1);
var s_tuple = await agent1.PeekAsync(s_template1);

await agent1.PeekAsync(s_template2, x =>
{
    Console.WriteLine(x);
    return Task.CompletedTask;
});

await agent1.WriteAsync(s_tuple2);

// IntTuple
var provider2 = client.ServiceProvider.GetRequiredService<ISpaceAgentProvider<int, IntTuple, IntTemplate>>();
var agent2 = await provider2.GetAsync();

IntTuple i_tuple1 = new(1, 2, 3);
IntTemplate i_template1 = new(1, null, 3);
IntTuple i_tuple2 = new(1, 2, 3, 4);
IntTemplate i_template2 = new(1, null, 3, null);

await agent2.WriteAsync(i_tuple1);
var i_tuple = await agent2.PeekAsync(i_template1);

await agent2.PeekAsync(i_template2, x =>
{
    Console.WriteLine(x);
    return Task.CompletedTask;
});


await agent2.WriteAsync(i_tuple2);

Console.WriteLine("\nPress any key to terminate...\n");
Console.ReadKey();

await host.StopAsync();