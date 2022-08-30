using OrleanSpaces;
using OrleanSpaces.Primitives;
using Microsoft.Extensions.Hosting;

public class Worker : BackgroundService
{
    private readonly ISpaceChannelProvider proxy;
    private readonly IHostApplicationLifetime lifetime;

    public Worker(
        ISpaceChannelProvider proxy,
        IHostApplicationLifetime lifetime)
    {
        this.proxy = proxy;
        this.lifetime = lifetime;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        ISpaceChannel channel = await proxy.GetAsync();

        Console.WriteLine("----------------------");
        Console.WriteLine("Type -u to unsubscribe.");
        Console.WriteLine("Type -r to see results.");
        Console.WriteLine("----------------------\n");

        var obsvRef = channel.Subscribe(new Ponger(channel));

        while (!cancellationToken.IsCancellationRequested)
        {
            Console.WriteLine("Type a message...");
            var message = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(message))
                continue;

            if (message == "-u")
            {
                channel.Unsubscribe(obsvRef);
                continue;
            }

            if (message == "-r")
                break;

            await channel.WriteAsync(SpaceTuple.Create((message, DateTime.Now)));
        }

        Console.WriteLine("----------------------\n");
        Console.WriteLine("Total tuples in space:\n");

        foreach (var tuple in await channel.ScanAsync(SpaceTemplate.Create((UnitField.Null, UnitField.Null))))
        {
            Console.WriteLine(tuple);
        }

        channel.Unsubscribe(obsvRef);

        Console.WriteLine("\nPress any key to terminate...");
        Console.ReadKey();

        lifetime.StopApplication();
    }
}