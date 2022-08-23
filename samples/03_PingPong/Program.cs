using Microsoft.Extensions.DependencyInjection;
using Orleans;
using OrleanSpaces.Clients;

var client = new ClientBuilder()
    .UseLocalhostClustering()
    .UseTupleSpace()
    .ConfigureServices(services =>
    {
        services.AddTransient<Pinger>();
        services.AddTransient<Ponger>();
    })
    .Build();

await client.Connect();

Console.WriteLine("Connected to the tuple space.\n\n");

await client.SubscribeAsync(sp => sp.GetRequiredService<Pinger>());
await client.SubscribeAsync(sp => sp.GetRequiredService<Ponger>());

var pinger = client.ServiceProvider.GetRequiredService<Pinger>();
var ponger = client.ServiceProvider.GetRequiredService<Ponger>();

await pinger.PingAsync();
await ponger.PongAsync();

Console.WriteLine("\n\nPress any key to terminate...\n\n");
Console.ReadLine();

await client.Close();