using Microsoft.Extensions.DependencyInjection;
using Orleans;
using OrleanSpaces.Clients;
using OrleanSpaces.Core.Primitives;

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

var pinger = client.ServiceProvider.GetRequiredService<Pinger>();
var ponger = client.ServiceProvider.GetRequiredService<Ponger>();

await client.SubscribeAsync(pinger);  // OR -> SubscribeAsync(sp => sp.GetRequiredService<Pinger>())
await client.SubscribeAsync(ponger);  // OR -> SubscribeAsync(sp => sp.GetRequiredService<Ponger>())

await pinger.PingAsync();
await ponger.PongAsync();

ConsoleKeyInfo input;

do
{
    input = Console.ReadKey();
} 
while (input.Key != ConsoleKey.Escape);

Console.WriteLine("\n\nPress any key to terminate...\n\n");
Console.ReadLine();

await client.Close();