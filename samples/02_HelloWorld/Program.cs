using Microsoft.Extensions.DependencyInjection;
using Orleans;
using OrleanSpaces.Clients;
using OrleanSpaces.Core.Primitives;

var client = new ClientBuilder()
    .UseLocalhostClustering()
    .UseTupleSpace()
    .Build();

await client.Connect();

Console.WriteLine("Connected to the tuple space.\n\n");

var spaceClient = client.ServiceProvider.GetRequiredService<ISpaceClient>();

const string EXCHANGE_KEY = "exchange-key";

var task1 = Task.Run(async () =>
{
    SpaceTuple tuple = SpaceTuple.Create((EXCHANGE_KEY, "Hey its thread 1"));
    await spaceClient.WriteAsync(tuple);

    Console.WriteLine($"THREAD 1: Placed '{tuple}' into the tuple space.");
    SpaceTemplate template = SpaceTemplate.Create((EXCHANGE_KEY, UnitField.Null, UnitField.Null));

    while (true)
    {
        Console.WriteLine($"THREAD 1: Searching for matching tuple with template: {template}");
        SpaceTuple? helloWorldTuple = await spaceClient.PeekAsync(template);

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
        SpaceTuple? helloTuple = await spaceClient.PeekAsync(template);
        if (helloTuple != null)
        {
            Console.WriteLine($"THREAD 2: Found this tuple: {helloTuple}");

            SpaceTuple helloWorldTuple = SpaceTuple.Create((helloTuple[0], helloTuple[1], "Whats up its thread 2"));
            await spaceClient.WriteAsync(helloWorldTuple);

            Console.WriteLine($"THREAD 2: Placed '{helloWorldTuple}' into the tuple space.");

            break;
        }

        await Task.Delay(1000);
    }
});

Task.WaitAll(task1, task2);

Console.WriteLine("\n\nPress any key to terminate...\n\n");
Console.ReadLine();

await client.Close();