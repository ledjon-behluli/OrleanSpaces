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
        ISpaceAgent agent = await channel.OpenAsync();

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
        await Task.Delay(5000);

        SpaceTuple tuple = new(EXCHANGE_KEY, 1.2334);
        Console.WriteLine($"WORKER: Writing sensor data in form of the tuple {tuple}");
        await agent.WriteAsync(tuple);

        while (!callbackExecuted)
        {
            Console.WriteLine("WORKER: Doing some other stuff.");
            await Task.Delay(100, cancellationToken);
        }

        Console.WriteLine($"WORKER: Checking if {tuple} is still in space: {!(await agent.PeekAsync(template)).IsPassive}");

        Console.WriteLine("\nPress any key to terminate...");
        Console.ReadKey();

        lifetime.StopApplication();
    }
}
