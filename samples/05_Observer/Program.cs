using OrleanSpaces;
using OrleanSpaces.Tuples;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

using var host = new HostBuilder()
    .ConfigureAppConfiguration(config => config.AddJsonFile("appsettings.json"))
    .ConfigureServices(services =>
    {
        services.AddTupleSpace();
        services.AddSingleton<Ponger>();
        services.AddSingleton<Auditor>();
        services.AddSingleton<Completer>();
        services.AddSingleton<Archiver>();
    })
    .UseOrleansClient(builder =>
    {
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

ISpaceAgentProvider provider = host.Services.GetRequiredService<ISpaceAgentProvider>();

Ponger ponger = host.Services.GetRequiredService<Ponger>();
Auditor auditor = host.Services.GetRequiredService<Auditor>();
Completer completer = host.Services.GetRequiredService<Completer>();
Archiver archiver = host.Services.GetRequiredService<Archiver>();

ISpaceAgent agent = await provider.GetAsync();

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

foreach (var tuple in await agent.ScanAsync(template))
{
    Console.WriteLine(tuple);
}

Console.WriteLine("----------------------\n");
Console.WriteLine("Removing all tuples from space to see observation...\n");

int count = await agent.CountAsync();
for (int i = 0; i < count; i++)
{
    await agent.PopAsync(template);
}

await Task.Delay(3000);   // Giving some time for Completer.cs to do its thing.

Console.WriteLine("\nPress any key to terminate...");
Console.ReadKey();

await host.StopAsync();