using Microsoft.Extensions.DependencyInjection;
using Orleans;
using OrleanSpaces.Clients;
using OrleanSpaces.Core.Primitives;

var client = new ClientBuilder()
    .UseLocalhostClustering()
    .UseTupleSpace()
    .Build();

await client.Connect();

Console.WriteLine("Connected to the tuple space.\n");
Console.WriteLine("Type -u to unsubscribe.");
Console.WriteLine("Type -r to see results.");

var spaceClient = client.ServiceProvider.GetRequiredService<ISpaceClient>();

var ponger = new Ponger(spaceClient);
var pongerRef = await client.SubscribeAsync(ponger);  // If IoC is used -> SubscribeAsync(sp => sp.GetRequiredService<Ponger>())

while (true)
{
    Console.WriteLine("Type a message...");
    var message = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(message))
        continue;

    if (message == "-u")
    {
        await client.UnsubscribeAsync(pongerRef);
        continue;
    }

    if (message == "-r")
        break;

    await spaceClient.WriteAsync(SpaceTuple.Create((message, DateTime.Now)));
}

Console.WriteLine("----------------------\n");
Console.WriteLine("Total tuples in space:\n");

foreach (var tuple in await spaceClient.ScanAsync(SpaceTemplate.Create((UnitField.Null, UnitField.Null))))
{
    Console.WriteLine(tuple);
}

await client.UnsubscribeAsync(pongerRef);
await client.Close();

Console.WriteLine("\nPress any key to terminate...");
Console.ReadKey();