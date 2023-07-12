using OrleanSpaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using OrleanSpaces.Tuples;
using OrleanSpaces.Tuples.Specialized;

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

// SpaceTuple

var agent1 = client.ServiceProvider.GetRequiredService<ISpaceAgent>();

SpaceTuple s_tuple1 = new(1, "2", 3);
SpaceTemplate s_template1 = new(1, null, 3);

//await agent1.WriteAsync(s_tuple1);

_ = Task.Run(async () =>
{
    await foreach (var tuple in agent1.ConsumeAsync())
    {
        Console.WriteLine($"PROD1: {tuple}");
    }
});

_ = Task.Run(async () =>
{
    await foreach (var tuple in agent1.ConsumeAsync())
    {
        Console.WriteLine($"PROD2: {tuple}");
    }
});

int i = 0;
while (true)
{
    await agent1.WriteAsync(new(i));
    await Task.Delay(1000);

    i++;
}


var t1 = await agent1.PopAsync(s_template1);

// IntTuple

var agent2 = client.ServiceProvider.GetRequiredService<IIntAgent>();

IntTuple i_tuple1 = new(1, 2, 3);
IntTemplate i_template1 = new(1, null, 3);

await agent2.WriteAsync(i_tuple1);
var t2 = await agent2.PopAsync(i_template1);

Console.WriteLine("\nPress any key to terminate...\n");
Console.ReadKey();

await host.StopAsync();