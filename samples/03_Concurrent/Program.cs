﻿using Orleans;
using OrleanSpaces;
using OrleanSpaces.Primitives;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Hosting;

var client = new ClientBuilder()
    .UseLocalhostClustering()
    .AddSimpleMessageStreamProvider(StreamNames.PubSubProvider)
    .AddTupleSpace()
    .Build();

await client.Connect();   // If not called explicitly, it is handle by the library.

Console.WriteLine("Connected to the tuple space.\n\n");

var channel = client.ServiceProvider.GetRequiredService<ISpaceChannel>();

//Normally you would call: "var agent = await channel.GetAsync();" somewhere here, but I want to showcase the thread-safety of the method.

const string EXCHANGE_KEY = "exchange-key";


await Task.WhenAll(CreateTasks(10, async index =>
{
    var agent = await channel.GetAsync();  // Only to showcase thread-safety (see comment above).
    var tuple = SpaceTuple.Create((EXCHANGE_KEY, index));

    await agent.WriteAsync(tuple);
    Console.WriteLine($"WRITER {index}: {tuple}");
}));


Console.WriteLine("----------------------");


await Task.WhenAll(CreateTasks(10, async index =>
{
    var agent = await channel.GetAsync();  // Only to showcase thread-safety (see comment above).
    var tuple = await agent.PeekAsync(SpaceTemplate.Create((EXCHANGE_KEY, index)));

    Console.WriteLine($"READER {index}: {tuple}");
}));


Console.WriteLine("\nPress any key to terminate...\n");
Console.ReadKey();

await client.Close();

Task[] CreateTasks(int count, Func<int, Task> func)
{
    var tasks = new Task[count];
    for (int i = 0; i < count; i++)
    {
        int index = i;
        tasks[i] = Task.Run(() => func(index));
    }

    return tasks;
}