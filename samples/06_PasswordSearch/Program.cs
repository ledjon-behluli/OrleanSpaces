using OrleanSpaces;
using System.Text.Json;
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

Console.WriteLine("-----------------------------------------------------");
Console.WriteLine("--------- Master / Slave Password Search ------------");
Console.WriteLine("-----------------------------------------------------\n");

string json = File.ReadAllText("data.json");
var hashPasswordPairs = JsonSerializer.Deserialize<Dictionary<string, string>>(json)
    ?? throw new InvalidOperationException("Make sure 'data.json' is in the current directory.");

int numOfPasswords = 0;
int numOfPairs = hashPasswordPairs.Count;

while (true)
{
    Console.Write($"Choose number of password to search for (max == {numOfPairs}): ");
    var message = Console.ReadLine();

    if (!int.TryParse(message, out int count))
    {
        Console.WriteLine("Enter a positive number.");
        continue;
    }

    if (count < 1)
    {
        Console.WriteLine("Number of passwords must be >= 1");
        continue;
    }

    if (count > numOfPairs)
    {
        Console.WriteLine($"Max number of passwords is {numOfPairs}");
    }

    numOfPasswords = count;
    break;
}

ISpaceAgentProvider provider = host.Services.GetRequiredService<ISpaceAgentProvider>();
ISpaceAgent agent = await provider.GetAsync();

List<Slave> slaves = new();
int size = Math.DivRem(hashPasswordPairs.Count, numOfPasswords, out int remainder);
int slaveCount = numOfPasswords + (remainder > 0 ? 1 : 0);

for (int i = 0; i < slaveCount; i++)
{
    Dictionary<string, string> partition;

    if (i < slaveCount - 1)
    {
        partition = hashPasswordPairs
            .Skip(i * size)
            .Take(size)
            .ToDictionary(pair => pair.Key, pair => pair.Value);
    }
    else
    {
        partition = hashPasswordPairs
            .Skip(i * size)
            .ToDictionary(pair => pair.Key, pair => pair.Value);
    }

    slaves.Add(new(i, agent, partition));
}

Console.WriteLine($"Total number of {slaves.Count} slaves has been picked.");
Console.WriteLine("-----------------------------------------------------\n");

Master master = new(agent, numOfPasswords, hashPasswordPairs.Keys.ToList());
_ = agent.Subscribe(master);

await master.RunAsync();
await Parallel.ForEachAsync(slaves, async (slave, ct) => await slave.RunAsync());

while (!master.IsDone)
{

}

Console.WriteLine("\n-------------------------------------------------------");
Console.WriteLine("----------------------- Results -----------------------\n");

foreach (var pair in master.HashPasswordPairs)
{
    Console.WriteLine($"Hash = {pair.Key} | Password: {pair.Value}");
}

Console.WriteLine("\nPress any key to terminate...");
Console.ReadKey();

await host.StopAsync();

public static class ExchangeKeys
{
    public const string PASSWORD_SEARCH = "PASSWORD_SEARCH";
    public const string PASSWORD_FOUND = "PASSWORD_FOUND";
}