﻿using OrleanSpaces.Primitives;
using Microsoft.Extensions.Hosting;
using OrleanSpaces;

public class Worker : BackgroundService
{
    private readonly ISpaceChannelProxy proxy;
    private readonly IHostApplicationLifetime lifetime;

    public Worker(
        ISpaceChannelProxy proxy,
        IHostApplicationLifetime lifetime)
    {
        this.proxy = proxy;
        this.lifetime = lifetime;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        ISpaceChannel channel = await proxy.OpenAsync(cancellationToken);

        Console.WriteLine("----------------------");
        Console.WriteLine("Type -u to unsubscribe.");
        Console.WriteLine("Type -r to see results.");
        Console.WriteLine("----------------------\n");

        var ponger = new Ponger(channel);
        var pongerRef = channel.Subscribe(ponger);

        while (!cancellationToken.IsCancellationRequested)
        {
            Console.WriteLine("Type a message...");
            var message = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(message))
                continue;

            if (message == "-u")
            {
                channel.Unsubscribe(pongerRef);
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

        channel.Unsubscribe(pongerRef);

        Console.WriteLine("\nPress any key to terminate...");
        Console.ReadKey();

        lifetime.StopApplication();
    }
}