﻿using Microsoft.Extensions.DependencyInjection;
using Orleans;
using OrleanSpaces.Clients;
using OrleanSpaces.Core.Primitives;

var client = new ClientBuilder()
    .UseLocalhostClustering()
    .UseTupleSpace()
    .Build();

await client.Connect();

Console.WriteLine("Connected to the tuple space.\n");

var spaceClient = client.ServiceProvider.GetRequiredService<ISpaceClient>();

var pinger = new Pinger(spaceClient);
var ponger = new Ponger(spaceClient);

await client.SubscribeAsync(pinger);  // If IoC is used -> SubscribeAsync(sp => sp.GetRequiredService<Pinger>())
await client.SubscribeAsync(ponger);  // If IoC is used -> SubscribeAsync(sp => sp.GetRequiredService<Ponger>())

await Kickstart();

await client.UnsubscribeAsync(pinger);
await client.UnsubscribeAsync(ponger);

Console.WriteLine("\n\nPress any key to terminate...\n\n");
Console.ReadKey();

await client.Close();

async Task Kickstart()
{
    await spaceClient.WriteAsync(SpaceTuple.Create(("Ping", "Kickstart")));

    while (true)
    {
        if (pinger.Iterations >= 10 && ponger.Iterations >= 10)
        {
            Console.WriteLine("\nTotal tuples in space:\n\n");

            foreach (var tuple in await spaceClient.ScanAsync(SpaceTemplate.CreateWithDefaults(2)))
            {
                Console.WriteLine($"{tuple}\n");
            }

            break;
        }

        //await Task.Delay(100);
    }
}