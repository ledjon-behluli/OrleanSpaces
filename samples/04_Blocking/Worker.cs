using OrleanSpaces;
using OrleanSpaces.Primitives;
using Microsoft.Extensions.Hosting;

public class Worker : BackgroundService
{
    private readonly ISpaceChannelProxy proxy;
    private readonly IHostApplicationLifetime lifetime;

    public Worker(
        ISpaceChannelProxy proxy,
        IHostApplicationLifetime lifetime)
    {
        this.proxy = proxy;
        this.lifetime = lifetime;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        ISpaceChannel channel = await proxy.OpenAsync();

        const string EXCHANGE_KEY = "Sensor 1";

        SpaceTemplate template = SpaceTemplate.Create((EXCHANGE_KEY, typeof(double)));
        Console.WriteLine($"READER: Peeking for a tuple that matches template {template} in a 'blocking' fashion...");

        await channel.PeekAsync(template, async tuple =>
        {
            Console.WriteLine($"READER: Got back response for my template {template} in form of this tuple '{tuple}'. Doing some 'heavy' work...");

            await Task.Delay(1000);

            Console.WriteLine("Done.");
        });


        Console.WriteLine($"\nSYSTEM: Simulating some delay until a tuple that matches template {template} is written...");
        await Task.Delay(5000, cancellationToken);

        SpaceTuple tuple = SpaceTuple.Create((EXCHANGE_KEY, 1.2334));
        Console.WriteLine($"\nWRITER: Writing sensor data in form of the tuple {tuple}");
        await channel.WriteAsync(tuple);


        Console.WriteLine("\nPress any key to terminate...");
        Console.ReadKey();

        lifetime.StopApplication();
    }
}