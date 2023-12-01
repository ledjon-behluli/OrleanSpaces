using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans.Configuration;
using OrleanSpaces;
using System.Net;

namespace SiloShared;

public static class Program
{
    public static async Task RunLoop(int silo)
    {
        int siloNum = silo - 1;

        IHost host = Host.CreateDefaultBuilder(null)
           .UseOrleans(siloBuilder =>
           {
               siloBuilder.UseLocalhostClustering(
                   siloPort: EndpointOptions.DEFAULT_SILO_PORT + siloNum,
                   gatewayPort: EndpointOptions.DEFAULT_GATEWAY_PORT + siloNum,
                   primarySiloEndpoint: new IPEndPoint(IPAddress.Loopback, EndpointOptions.DEFAULT_SILO_PORT));
               
               siloBuilder.AddOrleanSpaces(configureClientOptions: o => o.EnabledSpaces = SpaceKind.Generic);
               siloBuilder.AddMemoryStreams(Constants.PubSubProvider);
               siloBuilder.AddMemoryGrainStorage(Constants.PubSubStore);
               siloBuilder.AddMemoryGrainStorage(Constants.StorageName);
           })
           .Build();

        await host.StartAsync();

        Console.WriteLine($"Silo {silo} started.\n\n");

        var grain = host.Services.GetRequiredService<IGrainFactory>().GetGrain<ILocalCacheGrain>(0);

        while (true)
        {
            Console.WriteLine("Choose operation: 0 - GET, 1 - ADD, 2 - DELETE");
            string? action = Console.ReadLine();
            if (action is null)
            {
                Console.WriteLine("Operation must not be null.");
                continue;
            }

            if (!int.TryParse(action, out int op) || op > 2)
            {
                continue;
            }

            Console.WriteLine("Enter key:");
            string? key = Console.ReadLine();
            if (key is null)
            {
                Console.WriteLine("Key must not be null.");
                continue;
            }

            if (op == 0)
            {
                object? result = await grain.GetValue(key);
                Console.WriteLine(result is null ? "No value found" : $"Found value: {result}");
                continue;
            }
            
            if (op == 1)
            {
                Console.WriteLine("Enter value:");
                string? value = Console.ReadLine();
                if (value is null)
                {
                    Console.WriteLine("Value must not be null.");
                    continue;
                }

                await grain.Add(key, value);
                Console.WriteLine($"Added: [Key: {key} - Value: {value}]");
                continue;
            }

            if (op == 2)
            {
                object? result = await grain.RemoveEntry(key);
                Console.WriteLine(result is null ? $"Entry for key [{key}] does not exist" : $"Removed value: {result}");
                continue;
            }
        }
    }
}