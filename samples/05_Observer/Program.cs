﻿using OrleanSpaces;
using OrleanSpaces.Tuples;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

var host = new HostBuilder()
    .ConfigureAppConfiguration(config => config.AddJsonFile("appsettings.json"))
    .UseOrleansClient(builder =>
    {
        builder.AddOrleanSpaces();
        builder.UseLocalhostClustering();
        builder.AddMemoryStreams(Constants.PubSubProvider);
    })
    .ConfigureLogging((context, builder) =>
    {
        builder.ClearProviders();
        builder.AddConfiguration(context.Configuration.GetSection("Logging"));
        builder.AddConsole();
    })
    .Build();

await host.StartAsync();

Console.WriteLine("Connected to the tuple space.\n\n");

ISpaceAgent agent = host.Services.GetRequiredService<ISpaceAgent>();

Ponger ponger = new(agent);
Auditor auditor = new();
Completer completer = new();
Archiver archiver = new();

Console.WriteLine("----------------------");
Console.WriteLine("Type -u to unsubscribe.");
Console.WriteLine("Type -r to see results.");
Console.WriteLine("Type [Ping] to get back [Pong]");
Console.WriteLine("----------------------\n");

Guid pongerId = agent.Subscribe(ponger);
_ = agent.Subscribe(auditor);
_ = agent.Subscribe(completer);
_ = agent.Subscribe(archiver);

while (true)
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

SpaceTemplate template = new(null, null);

foreach (var tuple in agent.Enumerate(template))
{
    Console.WriteLine(tuple);
}

Console.WriteLine("----------------------\n");
Console.WriteLine("Removing all tuples from space to see observation...\n");

for (int i = 0; i < agent.Count; i++)
{
    await agent.PopAsync(template);
}

await Task.Delay(3000);   // Giving some time for Completer.cs to do its thing.

Console.WriteLine("\nPress any key to terminate...");
Console.ReadKey();

await host.StopAsync();