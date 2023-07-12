using OrleanSpaces;
using Microsoft.Extensions.DependencyInjection;
using OrleanSpaces.Tuples;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .UseOrleansClient(builder =>
    {
        builder.AddOrleanSpaces();
        builder.UseLocalhostClustering();
        builder.AddMemoryStreams(Constants.PubSubProvider);
    })
    .Build();

await host.StartAsync();

Console.WriteLine("Connected to the tuple space.\n\n");

ISpaceAgent agent = host.Services.GetRequiredService<ISpaceAgent>();

const string EXCHANGE_KEY = "exchange-key";

await Task.WhenAll(CreateTasks(10, async index =>
{
    SpaceTuple tuple = new(EXCHANGE_KEY, index);
    await agent.WriteAsync(tuple);
    Console.WriteLine($"WRITER {index}: {tuple}");
}));


Console.WriteLine("----------------------");


await Task.WhenAll(CreateTasks(10, async index =>
{
    SpaceTuple tuple = await agent.PeekAsync(new(EXCHANGE_KEY, index));
    Console.WriteLine($"READER {index}: {tuple}");
}));


Console.WriteLine("\nPress any key to terminate...\n");
Console.ReadKey();

await host.StopAsync();

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