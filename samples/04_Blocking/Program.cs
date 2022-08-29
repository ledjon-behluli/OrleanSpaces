using Orleans;
using OrleanSpaces;
using OrleanSpaces.Primitives;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Hosting;
using OrleanSpaces.Observers;

var client = new ClientBuilder()
    .UseLocalhostClustering()
    .AddSimpleMessageStreamProvider(StreamNames.PubSubProvider)
    .AddTupleSpace()
    .Build();

await client.Connect();   // If not called explicitly, it is handle by the library.

Console.WriteLine("Connected to the tuple space.\n\n");

var proxy = client.ServiceProvider.GetRequiredService<ISpaceChannelProxy>();
var channel = await proxy.OpenAsync();

const string EXCHANGE_KEY = "Sensor 1";

var oRef = channel.Subscribe(new Observer());

SpaceTemplate template = SpaceTemplate.Create((EXCHANGE_KEY, typeof(double)));
Console.WriteLine($"READER: Peeking for a tuple that matches template {template} in a 'blocking' fashion...");

await channel.PeekAsync(template, async tuple =>
{
    Console.WriteLine($"READER: Got back response for my template {template} in form of this tuple '{tuple}'. Doing some 'heavy' work...");

    await Task.Delay(1000);

    Console.WriteLine("Done.");
});


Console.WriteLine($"\nSYSTEM: Simulating some delay until a tuple that matches template {template} is written...");
await Task.Delay(5000);

SpaceTuple tuple = SpaceTuple.Create((EXCHANGE_KEY, 1.2334));
Console.WriteLine($"\nWRITER: Writing sensor data in form of the tuple {tuple}");
await channel.WriteAsync(tuple);


Console.WriteLine("\nPress any key to terminate...\n");
Console.ReadKey();

channel.Unsubscribe(oRef);
await client.Close();


class Observer : ISpaceObserver
{
    public Task OnTupleAsync(SpaceTuple tuple)
    {
        Console.WriteLine($"OBSERVER: {tuple}");
        return Task.CompletedTask;
    }
}