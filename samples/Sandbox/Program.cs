using OrleanSpaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using OrleanSpaces.Tuples;
using OrleanSpaces.Tuples.Typed;

var host = new HostBuilder()
    .UseOrleansClient(builder =>
    {
        builder.AddOrleanSpaces(options => options.EnabledSpaces = SpaceType.Generic | SpaceType.Ints);
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

await agent1.WriteAsync(s_tuple1);
var t1 = await agent1.PopAsync(s_template1);

// IntTuple

var provider2 = client.ServiceProvider.GetRequiredService<IIntAgentProvider>();
var agent2 = await provider2.GetAsync();

IntTuple i_tuple1 = new(1, 2, 3);
IntTemplate i_template1 = new(1, null, 3);

await agent2.WriteAsync(i_tuple1);
var t2 = await agent2.PopAsync(i_template1);

Console.WriteLine("\nPress any key to terminate...\n");
Console.ReadKey();

var provider3 = client.ServiceProvider.GetRequiredService
    <ISpaceAgentProvider<MyEnum, EnumTuple<MyEnum>, EnumTemplate<MyEnum>>>();

await host.StopAsync();


enum MyEnum
{

}