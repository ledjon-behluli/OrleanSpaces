using Orleans;
using OrleanSpaces;
using OrleanSpaces.Primitives;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Hosting;

var client = new ClientBuilder()
    .UseLocalhostClustering()
    .AddSimpleMessageStreamProvider(StreamNames.PubSubProvider)
    .AddTupleSpace()
    .Build();

await client.Connect();   // Comment: If not called explicitly, it is handle by the library.

Console.WriteLine("Connected to the tuple space.\n\n");

var proxy = client.ServiceProvider.GetRequiredService<ISpaceChannelProxy>();
var channel = await proxy.OpenAsync();

const string EXCHANGE_KEY = "exchange-key";

var task1 = Task.Run(async () =>
{
    SpaceTuple tuple = SpaceTuple.Create((EXCHANGE_KEY, "Hey its thread 1"));
    await channel.WriteAsync(tuple);

    Console.WriteLine($"THREAD 1: Placed '{tuple}' into the tuple space.");
    SpaceTemplate template = SpaceTemplate.Create((EXCHANGE_KEY, UnitField.Null, UnitField.Null));

    while (true)
    {
        Console.WriteLine($"THREAD 1: Searching for matching tuple with template: {template}");

        var helloWorldTuple = await channel.PeekAsync(template);
        if (helloWorldTuple != null)
        {
            Console.WriteLine($"THREAD 1: Found this tuple: {helloWorldTuple}");
            break;
        }

        await Task.Delay(1000);
    }
});

var task2 = Task.Run(async () =>
{
    SpaceTemplate template = SpaceTemplate.Create((EXCHANGE_KEY, UnitField.Null));
    Console.WriteLine($"THREAD 2: Searching for matching tuple with template: {template}");

    while (true)
    {
        var helloTuple = await channel.PeekAsync(template);
        if (helloTuple != null)
        {
            Console.WriteLine($"THREAD 2: Found this tuple: {helloTuple}");

            SpaceTuple helloWorldTuple = SpaceTuple.Create((helloTuple[0], helloTuple[1], "Whats up its thread 2"));
            await channel.WriteAsync(helloWorldTuple);

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


// TODO: Find me for testng proxy mutli-threading

//ISpaceChannelProxy proxy = client.ServiceProvider.GetRequiredService<ISpaceChannelProxy>();

//var tasks = new Task[100];
//for (int i = 0; i < 100; i++)
//{
//    tasks[i] = Task.Run(async () =>
//    {
//        await proxy.OpenAsync();
//    });
//}

//await Task.WhenAll(tasks);