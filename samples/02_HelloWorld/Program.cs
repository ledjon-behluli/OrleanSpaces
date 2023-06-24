using OrleanSpaces;
using Microsoft.Extensions.DependencyInjection;
using OrleanSpaces.Tuples;
using Microsoft.Extensions.Hosting;

using var host = new HostBuilder()
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

const string EXCHANGE_KEY = "exchange-key";

var task1 = Task.Run(async () =>
{
    SpaceTuple tuple = new(EXCHANGE_KEY, "Hey its thread 1");
    await agent.WriteAsync(tuple);

    Console.WriteLine($"THREAD 1: Placed '{tuple}' into the tuple space.");
    SpaceTemplate template = new(EXCHANGE_KEY, null, null);

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
});

var task2 = Task.Run(async () =>
{
    SpaceTemplate template = new(EXCHANGE_KEY, null);
    Console.WriteLine($"THREAD 2: Searching for matching tuple with template: {template}");

    while (true)
    {
        var helloTuple = await agent.PeekAsync(template);
        if (helloTuple.Length > 0)
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

await host.StopAsync();