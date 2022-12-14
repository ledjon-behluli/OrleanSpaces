using Orleans;
using OrleanSpaces;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Hosting;
using OrleanSpaces.Tuples;

var client = new ClientBuilder()
    .UseLocalhostClustering()
    .AddSimpleMessageStreamProvider(Constants.PubSubProvider)
    .AddTupleSpace()
    .Build();

await client.Connect();   // If not called explicitly, it is handle by the library.

Console.WriteLine("Connected to the tuple space.\n\n");

ISpaceAgentProvider provider = client.ServiceProvider.GetRequiredService<ISpaceAgentProvider>();

//Normally you would call "var agent = await provider.GetAsync();" somewhere here.
//But I want to showcase the thread-safety of the method.

const string EXCHANGE_KEY = "exchange-key";

await Task.WhenAll(CreateTasks(10, async index =>
{
    ISpaceAgent agent = await provider.GetAsync();  // Only to showcase thread-safety (see comment above).
    SpaceTuple tuple = new(EXCHANGE_KEY, index);

    await agent.WriteAsync(tuple);
    Console.WriteLine($"WRITER {index}: {tuple}");
}));


Console.WriteLine("----------------------");


await Task.WhenAll(CreateTasks(10, async index =>
{
    ISpaceAgent agent = await provider.GetAsync();  // Only to showcase thread-safety (see comment above).
    SpaceTuple tuple = await agent.PeekAsync(new(EXCHANGE_KEY, index));

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