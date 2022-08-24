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

var spaceClient = client.ServiceProvider.GetRequiredService<ISpaceClient>();

var pinger = new Pinger(spaceClient);
var ponger = new Ponger(spaceClient);

var pingerRef = await client.SubscribeAsync(pinger);  // If IoC is used -> SubscribeAsync(sp => sp.GetRequiredService<Pinger>())
var pongerRef = await client.SubscribeAsync(ponger);  // If IoC is used -> SubscribeAsync(sp => sp.GetRequiredService<Ponger>())

await Kickstart();

await client.UnsubscribeAsync(pingerRef);
await client.UnsubscribeAsync(pongerRef);

Console.WriteLine("\n\nPress any key to terminate...\n\n");
Console.ReadKey();

await client.Close();

async Task Kickstart()
{
    await spaceClient.WriteAsync(SpaceTuple.Create(("Ping", "Kickstart")));

    while (true)
    {
        if (pinger.IsDone && ponger.IsDone)
        {
            Console.WriteLine("\nTotal tuples in space:\n\n");

            foreach (var tuple in await spaceClient.ScanAsync(SpaceTemplate.Create((UnitField.Null, UnitField.Null))))
            {
                Console.WriteLine($"{tuple}\n");
            }

            break;
        }

        //await Task.Delay(100);
    }
}