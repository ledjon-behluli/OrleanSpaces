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
        bool callbackExecuted = false;

        SpaceTemplate template = new(EXCHANGE_KEY, typeof(double));
        Console.WriteLine($"WORKER: Pop-ing a tuple that matches template {template} in a callback fashion...");

        await agent.PopAsync(template, async tuple =>
        {
            Console.WriteLine($"CALLBACK: Got back response for template {template} in form of this tuple '{tuple}'. Doing some heavy work...");

            await Task.Delay(1000, cancellationToken);

            Console.WriteLine("CALLBACK: Done with my work.");
            callbackExecuted = true;
        });

        Console.WriteLine($"WORKER: Simulating some delay until a tuple that matches template {template} is written...");
        await Task.Delay(5000, cancellationToken);

        SpaceTuple tuple = new(EXCHANGE_KEY, 1.2334);
        Console.WriteLine($"WORKER: Writing sensor data in form of the tuple {tuple}");
        await agent.WriteAsync(tuple);

        while (!callbackExecuted)
        {
            Console.WriteLine("WORKER: Doing some other stuff.");
            await Task.Delay(100, cancellationToken);
        }

        Console.WriteLine($"WORKER: Checking if {tuple} is still in space: {(await agent.PeekAsync(template)).Length > 0}");

        Console.WriteLine("\nPress any key to terminate...");
        Console.ReadKey();

        lifetime.StopApplication();
    }
}
