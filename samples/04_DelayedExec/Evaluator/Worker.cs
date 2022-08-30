using OrleanSpaces;
using OrleanSpaces.Primitives;
using Microsoft.Extensions.Hosting;

public class Worker : BackgroundService
{
    private readonly ISpaceChannel channel;
    private readonly IHostApplicationLifetime lifetime;

    public Worker(
        ISpaceChannel channel,
        IHostApplicationLifetime lifetime)
    {
        this.channel = channel;
        this.lifetime = lifetime;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        ISpaceAgent agent = await channel.GetAsync();

        const string EXCHANGE_KEY = "sensor-data";
        bool evalExecuted = false;

        Console.WriteLine($"WORKER: Evaluating new tuple...");

        await agent.EvaluateAsync(async () =>
        {
            Console.WriteLine("EVALUATOR: Doing so heavy work...");

            await Task.Delay(5000);
            SpaceTuple tuple = SpaceTuple.Create((EXCHANGE_KEY, 1.234));

            Console.WriteLine($"EVALUATOR: Returning tuple {tuple}");
            evalExecuted = true;

            return tuple;
        });

        while (!evalExecuted)
        {
            Console.WriteLine("WORKER: Doing some other stuff.");
            await Task.Delay(500);
        }

        Console.WriteLine($"WORKER: Result from evaluation: {await agent.PeekAsync(SpaceTemplate.Create((EXCHANGE_KEY, typeof(double))))}");

        Console.WriteLine("\nPress any key to terminate...");
        Console.ReadKey();

        lifetime.StopApplication();
    }
}