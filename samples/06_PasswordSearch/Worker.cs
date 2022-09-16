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
        Console.WriteLine("-------------------------------------------------");
        Console.WriteLine("-------- Password Searching Example -------------");
        Console.WriteLine("-------------------------------------------------\n");

        int noOfPasswords = 0;

        while (!cancellationToken.IsCancellationRequested)
        {
            Console.Write("Choose number of password to search for: ");
            var message = Console.ReadLine();

            if (!uint.TryParse(message, out uint count))
            {
                Console.WriteLine("Enter a positive number.");
                continue;
            }

            if (count < 1)
            {
                Console.WriteLine("Number of passwords must be >= 1");
                continue;
            }

            noOfPasswords = (int)count;
            break;
        }

        ISpaceAgent agent = await channel.OpenAsync();

        string json = File.ReadAllText("data.json");
        var hashPasswordPairs = JsonSerializer.Deserialize<Dictionary<string, string>>(json);

        if (hashPasswordPairs == null)
        {
            throw new InvalidOperationException("Make sure 'data.json' is in the current directory.");
        }

        Master master = new(agent, noOfPasswords, hashPasswordPairs.Keys.ToList());
        _ = agent.Subscribe(master);

        int size = Math.DivRem(hashPasswordPairs.Count, noOfPasswords, out int remainder);
        int rounds = noOfPasswords + (remainder > 0 ? 1 : 0);

        //Slave[] slaves = new Slave[rounds];

        //for (int i = 0; i < rounds; i++)
        //{
        //    var partition = hashPasswordPairs
        //        .Skip(i * size)
        //        .Take(size)
        //        .ToDictionary(pair => pair.Key, pair => pair.Value);

        //    Slave slave = new(i, agent, partition);
        //    _ = agent.Subscribe(slave);

        //    slaves[i] = slave;
        //}

        Slave slave = new(0, agent, hashPasswordPairs);
        _ = agent.Subscribe(slave);

        Stopwatch sw = new();
        sw.Start();

        await master.StartAsync();
        while (!master.IsDone || !cancellationToken.IsCancellationRequested) { }

        sw.Stop();

        Console.WriteLine("\n-------------------------------------------------\n");
        Console.WriteLine($"Results (total time: {sw.ElapsedMilliseconds})\n");

        foreach (var pair in master.HashPasswordPairs)
        {
            Console.WriteLine($"Hash = {pair.Key} | Password: {pair.Value}");
        }

        Console.WriteLine("\nPress any key to terminate...");
        Console.ReadKey();

        lifetime.StopApplication();
    }
}