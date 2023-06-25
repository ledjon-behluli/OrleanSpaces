using OrleanSpaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using OrleanSpaces.Tuples;

var host = new HostBuilder()
    .ConfigureServices(services => services.AddTupleSpace())
    .UseOrleansClient(builder =>
    {
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
await agent.WriteAsync(tuple);

SpaceTemplate template = new(1, null, 3);
Console.WriteLine($"Searching for matching tuple with template: {template}");

var _tuple = await agent.PeekAsync(template);
if (_tuple.Length > 0)
{
    Console.WriteLine($"Found this tuple: {_tuple}");
}

_tuple = await agent.PopAsync(template);
if (_tuple.Length > 0)
{
    Console.WriteLine($"Found this tuple: {_tuple} and removed it");
}

_tuple = await agent.PeekAsync(template);
if (_tuple.Length == 0)
{
    Console.WriteLine($"Tuple: {_tuple} has been removed");
}

Console.WriteLine("\nPress any key to terminate...\n");
Console.ReadKey();

await host.StopAsync();