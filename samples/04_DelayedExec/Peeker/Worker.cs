﻿using OrleanSpaces;
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
        bool callbackExecuted = false;

        SpaceTemplate template = SpaceTemplate.Create((EXCHANGE_KEY, typeof(double)));
        Console.WriteLine($"WORKER: Peek-ing a tuple that matches template {template} in a 'blocking' fashion...");

        await agent.PeekAsync(template, async tuple =>
        {
            Console.WriteLine($"CALLBACK: Got back response for template {template} in form of this tuple '{tuple}'. Doing some heavy work...");

            await Task.Delay(1000);

            Console.WriteLine("CALLBACK: Done with my work.");
            callbackExecuted = true;
        });

        Console.WriteLine($"WORKER: Simulating some delay until a tuple that matches template {template} is written...");
        await Task.Delay(5000);

        SpaceTuple tuple = SpaceTuple.Create((EXCHANGE_KEY, 1.2334));
        Console.WriteLine($"WORKER: Writing sensor data in form of the tuple {tuple}");
        await agent.WriteAsync(tuple);

        while (!callbackExecuted)
        {
            Console.WriteLine("WORKER: Doing some other stuff.");
            await Task.Delay(100);
        }

        Console.WriteLine($"WORKER: Checking if {tuple} is still in space: {!(await agent.PeekAsync(template)).IsEmpty}");

        Console.WriteLine("\nPress any key to terminate...");
        Console.ReadKey();

        lifetime.StopApplication();
    }
}