using Orleans.Hosting;
using Microsoft.Extensions.Logging;
using OrleanSpaces;
using Microsoft.Extensions.Hosting;
using Orleans;

var host = new SiloHostBuilder()
    .UseLocalhostClustering()
#region Streaming
    .AddSimpleMessageStreamProvider(Constants.PubSubProvider)
    //.AddAzureQueueStreams(Constants.PubSubProvider, Configs.QueueConfig)
#endregion
#region Persistence
    .AddMemoryGrainStorage(Constants.PubSubStore)
    .AddMemoryGrainStorage(Constants.SpaceStorage)
    .AddMemoryGrainStorage(Constants.IntStorage)

    //.AddAzureTableGrainStorage(Constants.PubSubStore, Configs.TableConfig)
    //.AddAzureTableGrainStorage(Constants.TupleSpaceStore, Configs.TableConfig)

    //.AddAzureBlobGrainStorage(Constants.PubSubStore, Configs.BlobConfig)
    //.AddAzureBlobGrainStorage(Constants.TupleSpaceStore, Configs.BlobConfig)

    //.AddAdoNetGrainStorage(Constants.PubSubStore, Configs.AdoNetConfig)
    //.AddAdoNetGrainStorage(Constants.TupleSpaceStore, Configs.AdoNetConfig)
#endregion
    .AddTupleSpace()
    .ConfigureLogging(builder => builder.AddConsole())
    .Build();

await host.StartAsync();

Console.WriteLine("\nPress any key to terminate...\n");
Console.ReadLine();

await host.StopAsync();
