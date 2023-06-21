using Orleans;
using OrleanSpaces;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Hosting;
using OrleanSpaces.Tuples;

var client = new ClientBuilder()
    .UseLocalhostClustering()
    .AddSimpleMessageStreamProvider(Constants.PubSubProvider)
    .AddTupleSpace()
    .Build();

await client.Connect();  

ISpaceAgentProvider provider = client.ServiceProvider.GetRequiredService<ISpaceAgentProvider>();
ISpaceAgent agent = await provider.GetAsync();

SpaceTuple tuple = new(1, 2, 3);
await agent.WriteAsync(tuple);

SpaceTemplate template = new(1, null, 3);

while (true)
{
    Console.WriteLine($"THREAD 1: Searching for matching tuple with template: {template}");

    var helloWorldTuple = await agent.PeekAsync(template);
    if (helloWorldTuple.Length > 0)
    {
        Console.WriteLine($"THREAD 1: Found this tuple: {helloWorldTuple}");
        break;
    }

    await Task.Delay(1000);
}

Console.ReadKey();

await client.Close();