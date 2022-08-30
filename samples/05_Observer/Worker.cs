using OrleanSpaces;
using OrleanSpaces.Primitives;
using Microsoft.Extensions.Hosting;

public class Worker : BackgroundService
{
    private readonly Ponger ponger;
    private readonly ISpaceChannel channel;
    private readonly IHostApplicationLifetime lifetime;

    public Worker(
        Ponger ponger,
        ISpaceChannel channel,
        IHostApplicationLifetime lifetime)
    {
        this.ponger = ponger;
        this.channel = channel;
        this.lifetime = lifetime;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        ISpaceAgent agent = await channel.GetAsync();

        Console.WriteLine("----------------------");
        Console.WriteLine("Type -u to unsubscribe.");
        Console.WriteLine("Type -r to see results.");
        Console.WriteLine("----------------------\n");

        var obsvRef = agent.Subscribe(ponger);

        while (!cancellationToken.IsCancellationRequested)
        {
            Console.WriteLine("Type a message...");
            var message = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(message))
                continue;

            if (message == "-u")
            {
                agent.Unsubscribe(obsvRef);
                continue;
            }

            if (message == "-r")
                break;

            await agent.WriteAsync(SpaceTuple.Create((message, DateTime.Now)));
        }

        Console.WriteLine("----------------------\n");
        Console.WriteLine("Total tuples in space:\n");

        foreach (var tuple in await agent.ScanAsync(SpaceTemplate.Create((UnitField.Null, UnitField.Null))))
        {
            Console.WriteLine(tuple);
        }

        agent.Unsubscribe(obsvRef);

        Console.WriteLine("\nPress any key to terminate...");
        Console.ReadKey();

        lifetime.StopApplication();
    }
}