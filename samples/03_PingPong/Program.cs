using Microsoft.Extensions.DependencyInjection;
using Orleans;
using OrleanSpaces.Clients;
using OrleanSpaces.Core.Primitives;

var client = new ClientBuilder()
    .UseLocalhostClustering()
    .UseTupleSpace()
    //.ConfigureServices(services =>
    //{
    //    services.AddSingleton<Pinger>();
    //    services.AddSingleton<Ponger>();
    //})
    .Build();

await client.Connect();

Console.WriteLine("Connected to the tuple space.\n");

var spaceClient = client.ServiceProvider.GetRequiredService<ISpaceClient>();

//await Task.Run(async () =>
//{
//    await client.SubscribeAsync(sp => sp.GetRequiredService<Pinger>());
//    await client.SubscribeAsync(sp => sp.GetRequiredService<Ponger>());

//    await spaceClient.WriteAsync(SpaceTuple.Create(("Ping", "Start")));
//});

var pinger = new Pinger(spaceClient);
var ponger = new Ponger(spaceClient);

await client.SubscribeAsync(pinger);  // If IoC is used -> SubscribeAsync(sp => sp.GetRequiredService<Pinger>())
await client.SubscribeAsync(ponger);  // If IoC is used -> SubscribeAsync(sp => sp.GetRequiredService<Ponger>())

await spaceClient.WriteAsync(SpaceTuple.Create((Constants.EXCHANGE_KEY, "Ping")));

while (true)
{
    if (pinger.Iterations == 10 && ponger.Iterations == 10)
    {
        var tuples = await spaceClient.ScanAsync(SpaceTemplate.CreateWithDefaults(2));

        Console.WriteLine("\nTotal tuples in space:\n");

        foreach (var tuple in tuples)
        {
            Console.WriteLine(tuple);
        }

        break;
    }
}

Console.WriteLine("\n\nPress any key to terminate...\n\n");
Console.ReadKey();

await client.Close();