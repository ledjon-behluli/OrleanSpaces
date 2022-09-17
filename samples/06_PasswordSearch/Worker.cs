using OrleanSpaces;
using Microsoft.Extensions.Hosting;
using System.Text.Json;
using System.Diagnostics;

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

            Console.WriteLine($"\nMulti-slave mode is {(isMultiSlaveMode ? "ON" : "OFF")}\n");

            numOfPasswords = count;
            break;
        }

        ISpaceAgent agent = await channel.OpenAsync();
        
        Master master = new(agent, numOfPasswords, hashPasswordPairs.Keys.ToList());
        _ = agent.Subscribe(master);

        List<Task> tasks = new();
        CancellationTokenSource cts = new();

        if (isMultiSlaveMode)
        {
            int size = Math.DivRem(hashPasswordPairs.Count, numOfPasswords, out int remainder);
            int rounds = numOfPasswords + (remainder > 0 ? 1 : 0);

            for (int i = 0; i < rounds; i++)
            {
                var partition = hashPasswordPairs
                    .Skip(i * size)
                    .Take(size)
                    .ToDictionary(pair => pair.Key, pair => pair.Value);

                Slave slave = new(i, agent, partition);
                tasks.Add(Task.Run(async () => await slave.RunAsync(cts.Token), cts.Token));
            }
        }
        else
        {
            Slave slave = new(0, agent, hashPasswordPairs);
            tasks.Add(Task.Run(async () => await slave.RunAsync(cts.Token), cts.Token));
        }

        Stopwatch sw = new();

        try
        {
            tasks.Add(Task.Run(() =>
            {
                while (!master.IsDone) { }
                cts.Cancel();
            }, cancellationToken));

           
            sw.Start();

            await master.RunAsync();
            await Task.WhenAll(tasks);
        }
        catch (OperationCanceledException)
        {

        }

        sw.Stop();

        Console.WriteLine("\n-----------------------------------------------------\n");
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