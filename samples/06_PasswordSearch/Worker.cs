using OrleanSpaces;
using Microsoft.Extensions.Hosting;
using System.Text.Json;
using System.Diagnostics;
using System.Reflection;

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
        Console.WriteLine("-----------------------------------------------------");
        Console.WriteLine("--------- Master / Slave Password Search ------------");
        Console.WriteLine("-----------------------------------------------------\n");

        string json = File.ReadAllText("data.json");
        var hashPasswordPairs = JsonSerializer.Deserialize<Dictionary<string, string>>(json)
            ?? throw new InvalidOperationException("Make sure 'data.json' is in the current directory.");

        bool isMultiSlaveMode = false;
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

            Console.Write($"Run in multi-slave mode (y/n): ");
            message = Console.ReadLine();

            if (message == "Y" || message == "y")
            {
                isMultiSlaveMode = true;
            }

            Console.WriteLine($"Multi-slave mode is {(isMultiSlaveMode ? "ON" : "OFF")}");

            numOfPasswords = count;
            break;
        }

        ISpaceAgent agent = await channel.OpenAsync();
        
        List<Slave> slaves = new();

        if (!isMultiSlaveMode)
        {
            slaves.Add(new(0, agent, hashPasswordPairs));
        }
        else
        {
            int size = Math.DivRem(hashPasswordPairs.Count, numOfPasswords, out int remainder);
            int slaveCount = numOfPasswords + (remainder > 0 ? 1 : 0);
            
            //if (slaveCount > Environment.ProcessorCount)
            //{
            //    slaveCount = Environment.ProcessorCount;
            //}

            for (int i = 0; i < slaveCount; i++)
            {
                var partition = hashPasswordPairs
                    .Skip(i * size)
                    .Take(size)
                    .ToDictionary(pair => pair.Key, pair => pair.Value);

                slaves.Add(new(i, agent, partition));
            }
        }

        Console.WriteLine($"Total number of {slaves.Count} slaves has been picked.");
        Console.WriteLine("-----------------------------------------------------\n");

        Master master = new(agent, numOfPasswords, hashPasswordPairs.Keys.ToList());
        _ = agent.Subscribe(master);

        Stopwatch sw = new();
        sw.Start();

        await master.RunAsync();

        await Parallel.ForEachAsync(slaves, async (slave, ct) => await slave.RunAsync());

        sw.Stop();

        Console.WriteLine("\n-----------------------------------------------------");
        Console.WriteLine($"Results (total time: {sw.ElapsedMilliseconds}ms)\n");

        foreach (var pair in master.HashPasswordPairs)
        {
            Console.WriteLine($"Hash = {pair.Key} | Password: {pair.Value}");
        }

        Console.WriteLine("\nPress any key to terminate...");
        Console.ReadKey();

        lifetime.StopApplication();
    }
}