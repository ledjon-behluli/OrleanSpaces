﻿using OrleanSpaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using OrleanSpaces.Tuples;

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
    await Task.Delay(500);
}

Console.WriteLine($"WORKER: Result from evaluation: {agent.Peek(new(EXCHANGE_KEY, typeof(double)))}");

Console.WriteLine("\nPress any key to terminate...");
Console.ReadKey();

await host.StopAsync();