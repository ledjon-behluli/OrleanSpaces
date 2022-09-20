using Orleans;
using OrleanSpaces;
using OrleanSpaces.Primitives;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Hosting;

var client = new ClientBuilder()
    .UseLocalhostClustering()
    .AddSimpleMessageStreamProvider(Constants.PubSubProvider)
    .AddTupleSpace()
    .Build();

await client.Connect();   // If not called explicitly, it is handle by the library.

Console.WriteLine("Connected to the tuple space.\n\n");

ISpaceAgentProvider provider = client.ServiceProvider.GetRequiredService<ISpaceAgentProvider>();
ISpaceAgent agent = await provider.GetAsync();

const string EXCHANGE_KEY = "exchange-key";

var task1 = Task.Run(async () =>
{
    SpaceTuple tuple = new(EXCHANGE_KEY, "Hey its thread 1");
    await agent.WriteAsync(tuple);

    Console.WriteLine($"THREAD 1: Placed '{tuple}' into the tuple space.");
    SpaceTemplate template = new(EXCHANGE_KEY, SpaceUnit.Null, SpaceUnit.Null);

    while (true)
    {
        Console.WriteLine($"THREAD 1: Searching for matching tuple with template: {template}");

        var helloWorldTuple = await agent.PeekAsync(template);
        if (!helloWorldTuple.IsPassive)
        {
            Console.WriteLine($"THREAD 1: Found this tuple: {helloWorldTuple}");
            break;
        }

        await Task.Delay(1000);
    }
});

var task2 = Task.Run(async () =>
{
    SpaceTemplate template = new(EXCHANGE_KEY, SpaceUnit.Null);
    Console.WriteLine($"THREAD 2: Searching for matching tuple with template: {template}");

    while (true)
    {
        var helloTuple = await agent.PeekAsync(template);
        if (!helloTuple.IsPassive)
        {
            Console.WriteLine($"THREAD 2: Found this tuple: {helloTuple}");

            SpaceTuple helloWorldTuple = new(helloTuple[0], helloTuple[1], "Whats up its thread 2");
            await agent.WriteAsync(helloWorldTuple);

            Console.WriteLine($"THREAD 2: Placed '{helloWorldTuple}' into the tuple space.");

            break;
        }

        await Task.Delay(1000);
    }
});

await Task.WhenAll(task1, task2);

Console.WriteLine("\nPress any key to terminate...\n");
Console.ReadKey();

await client.Close();