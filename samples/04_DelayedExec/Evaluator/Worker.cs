using OrleanSpaces;
using Microsoft.Extensions.Hosting;
using OrleanSpaces.Tuples;

public class Worker : BackgroundService
{
    private readonly ISpaceAgentProvider provider;
    private readonly IHostApplicationLifetime lifetime;

    public Worker(
        ISpaceAgentProvider provider,
        IHostApplicationLifetime lifetime)
    {
        this.provider = provider;
        this.lifetime = lifetime;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        ISpaceAgent agent = await provider.GetAsync();

        const string EXCHANGE_KEY = "sensor-data";
        bool evalExecuted = false;

        Console.WriteLine($"WORKER: Evaluating new tuple...");

        await agent.EvaluateAsync(async () =>
        {
            Console.WriteLine("EVALUATOR: Doing so heavy work...");

            await Task.Delay(5000);
            SpaceTuple tuple = new(EXCHANGE_KEY, 1.234);

            Console.WriteLine($"EVALUATOR: Returning tuple {tuple}");
            evalExecuted = true;

            return tuple;
        });

        while (!evalExecuted)
        {
            Console.WriteLine("WORKER: Doing some other stuff.");
            await Task.Delay(500, cancellationToken);
        }

        Console.WriteLine($"WORKER: Result from evaluation: {await agent.PeekAsync(new(EXCHANGE_KEY, typeof(double)))}");

        Console.WriteLine("\nPress any key to terminate...");
        Console.ReadKey();

        lifetime.StopApplication();
    }
}