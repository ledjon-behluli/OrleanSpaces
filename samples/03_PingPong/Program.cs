﻿using Orleans;
using OrleanSpaces;
using OrleanSpaces.Primitives;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Hosting;

var client = new ClientBuilder()
    .UseLocalhostClustering()
    .UseTupleSpace()
    .AddSimpleMessageStreamProvider(StreamNames.PubSubProvider)
    .Build();

await client.Connect();

Console.WriteLine("Connected to the tuple space.\n");
Console.WriteLine("Type -u to unsubscribe.");
Console.WriteLine("Type -r to see results.");
Console.WriteLine("----------------------\n");

var spaceClient = client.ServiceProvider.GetRequiredService<ISpaceClient>();

var ponger = new Ponger(spaceClient);
var pongerRef = spaceClient.Subscribe(ponger);

while (true)
{
    Console.WriteLine("Type a message...");
    var message = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(message))
        continue;

    if (message == "-u")
    {
        spaceClient.Unsubscribe(pongerRef);
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

spaceClient.Unsubscribe(pongerRef);
await client.Close();

Console.WriteLine("\nPress any key to terminate...");
Console.ReadKey();