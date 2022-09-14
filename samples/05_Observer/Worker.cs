using OrleanSpaces;
using OrleanSpaces.Primitives;
using Microsoft.Extensions.Hosting;

public class Worker : BackgroundService
{
    private readonly Ponger ponger;
    private readonly Auditor auditor;
    private readonly Completer completer;
    private readonly ISpaceChannel channel;
    private readonly IHostApplicationLifetime lifetime;

    public Worker(
        Ponger ponger,
        Auditor auditor,
        Completer completer,
        ISpaceChannel channel,
        IHostApplicationLifetime lifetime)
    {
        this.ponger = ponger;
        this.auditor = auditor;
        this.completer = completer;
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

        var pongerRef = agent.Subscribe(ponger);
        var auditorRef = agent.Subscribe(auditor);
        var completerRef = agent.Subscribe(completer);

        while (!cancellationToken.IsCancellationRequested)
        {
            Console.WriteLine("Type a message...");
            var message = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(message))
                continue;

            if (message == "-u")
            {
                agent.Unsubscribe(pongerRef);
                continue;
            }

            if (message == "-r")
                break;

            await agent.WriteAsync(new((message, DateTime.Now)));
        }

        Console.WriteLine("----------------------\n");
        Console.WriteLine("Total tuples in space:\n");

        SpaceTemplate template = new((SpaceUnit.Null, SpaceUnit.Null));

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

        agent.Unsubscribe(pongerRef);
        agent.Unsubscribe(auditorRef);
        agent.Unsubscribe(completerRef);

        Console.WriteLine("\nPress any key to terminate...");
        Console.ReadKey();

        lifetime.StopApplication();
    }
}