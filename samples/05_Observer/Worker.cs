using OrleanSpaces;
using Microsoft.Extensions.Hosting;
using OrleanSpaces.Tuples;

public class Worker : BackgroundService
{
    private readonly Ponger ponger;
    private readonly Auditor auditor;
    private readonly Completer completer;
    private readonly Archiver archiver;
    private readonly ISpaceAgentProvider provider;
    private readonly IHostApplicationLifetime lifetime;

    public Worker(
        Ponger ponger,
        Auditor auditor,
        Completer completer,
        Archiver archiver,
        ISpaceAgentProvider provider,
        IHostApplicationLifetime lifetime)
    {
        this.ponger = ponger;
        this.auditor = auditor;
        this.completer = completer;
        this.archiver = archiver;
        this.provider = provider;
        this.lifetime = lifetime;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        ISpaceAgent agent = await provider.GetAsync();

        Console.WriteLine("----------------------");
        Console.WriteLine("Type -u to unsubscribe.");
        Console.WriteLine("Type -r to see results.");
        Console.WriteLine("Type [Ping] to get back [Pong]");
        Console.WriteLine("----------------------\n");

        Guid pongerId = agent.Subscribe(ponger);
        _ = agent.Subscribe(auditor);
        _ = agent.Subscribe(completer);
        _ = agent.Subscribe(archiver);

        while (!cancellationToken.IsCancellationRequested)
        {
            Console.WriteLine("Type a message...");
            var message = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(message))
                continue;

            if (message == "-u")
            {
                agent.Unsubscribe(pongerId);
                continue;
            }

            if (message == "-r")
                break;

            await agent.WriteAsync(new(message, DateTime.Now));
        }

        Console.WriteLine("----------------------\n");
        Console.WriteLine("Total tuples in space:\n");

        SpaceTemplate template = new(new SpaceUnit(), new SpaceUnit());

        foreach (var tuple in await agent.ScanAsync(template))
        {
            Console.WriteLine(tuple);
        }

        Console.WriteLine("----------------------\n");
        Console.WriteLine("Removing all tuples from space to see observation...\n");

        int count = await agent.CountAsync();
        for (int i = 0; i < count; i++)
        {
            await agent.PopAsync(template);
        }

        await Task.Delay(3000, cancellationToken);   // Giving some time for Completer.cs to do its thing.

        Console.WriteLine("\nPress any key to terminate...");
        Console.ReadKey();

        lifetime.StopApplication();
    }
}