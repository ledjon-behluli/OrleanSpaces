using OrleanSpaces;
using OrleanSpaces.Primitives;
using Microsoft.Extensions.Hosting;

public class ConsoleClient : BackgroundService
{
    private readonly ISpaceClient client;
    private readonly IHostApplicationLifetime lifetime;

    public ConsoleClient(
        ISpaceClient client,
        IHostApplicationLifetime lifetime)
    {
        this.client = client;
        this.lifetime = lifetime;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Type -u to unsubscribe.");
        Console.WriteLine("Type -r to see results.");
        Console.WriteLine("----------------------\n");

        var ponger = new Ponger(client);
        var pongerRef = client.Subscribe(ponger);

        while (!cancellationToken.IsCancellationRequested)
        {
            Console.WriteLine("Type a message...");
            var message = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(message))
                continue;

            if (message == "-u")
            {
                client.Unsubscribe(pongerRef);
                continue;
            }

            if (message == "-r")
                break;

            await client.WriteAsync(SpaceTuple.Create((message, DateTime.Now)));
        }

        Console.WriteLine("----------------------\n");
        Console.WriteLine("Total tuples in space:\n");

        foreach (var tuple in await client.ScanAsync(SpaceTemplate.Create((UnitField.Null, UnitField.Null))))
        {
            Console.WriteLine(tuple);
        }

        client.Unsubscribe(pongerRef);

        Console.WriteLine("\nPress any key to terminate...");
        Console.ReadKey();

        lifetime.StopApplication();
    }
}