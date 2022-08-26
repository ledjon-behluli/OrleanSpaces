using Orleans.Hosting;
using Microsoft.Extensions.Logging;
using OrleanSpaces;

var host = new SiloHostBuilder()
    .UseLocalhostClustering()
    .AddTupleSpace()
    .AddSimpleMessageStreamProvider(StreamNames.PubSubProvider)
    .AddMemoryGrainStorage(StreamNames.PubSubStore)
    .AddMemoryGrainStorage(StorageNames.TupleSpaceStore)
    .ConfigureLogging(builder => builder.AddConsole())
    .Build();

await host.StartAsync();

Console.WriteLine("\n\nPress any key to terminate...\n\n");
Console.ReadLine();

await host.StopAsync();



//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;
//using Orleans.Hosting;
//using Orleans.Runtime;
//using OrleanSpaces;

//IHost host = Host.CreateDefaultBuilder(args)
//    .ConfigureAppConfiguration((ctx, builder) =>
//    {
//        builder
//            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
//            .AddJsonFile("appsettings.json");
//    })
//    .UseOrleans((ctx, siloBuilder) =>
//    {
//        siloBuilder
//            .AddStartupTask<HostStartup>()
//            .UseLocalhostClustering()
//            .AddTupleSpace()
//            .AddSimpleMessageStreamProvider(StreamNames.PubSubProvider)
//            .AddMemoryGrainStorage(StreamNames.PubSubStore)
//            .AddMemoryGrainStorage(StorageNames.TupleSpaceStore);
//    })
//    .ConfigureLogging(builder => builder.AddConsole())
//    .UseConsoleLifetime()
//    .Build();

//await host.RunAsync();

//class HostStartup : IStartupTask
//{
//    public Task Execute(CancellationToken cancellationToken)
//    {
//        Console.WriteLine("Press any key to terminate...");
//        return Task.CompletedTask;
//    }
//}