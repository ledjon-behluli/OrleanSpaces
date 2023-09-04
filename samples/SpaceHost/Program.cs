using OrleanSpaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .UseOrleans(builder =>
    {
        builder.AddOrleanSpaces();
        builder.UseLocalhostClustering();
        #region Streaming

        builder.AddMemoryStreams(Constants.PubSubProvider);
        //builder.AddAzureQueueStreams(Constants.PubSubProvider, Configs.QueueConfig);

        #endregion
        #region Persistence

        builder.AddMemoryGrainStorage(Constants.PubSubStore);
        builder.AddMemoryGrainStorage(Constants.StorageName);

        //builder.AddAzureTableGrainStorage(Constants.PubSubStore, Configs.TableConfig);
        //builder.AddAzureTableGrainStorage(Constants.StorageName, Configs.TableConfig);

        //builder.AddAzureBlobGrainStorage(Constants.PubSubStore, Configs.BlobConfig);
        //builder.AddAzureBlobGrainStorage(Constants.StorageName, Configs.BlobConfig);

        //builder.AddAdoNetGrainStorage(Constants.PubSubStore, Configs.AdoNetConfig);
        //builder.AddAdoNetGrainStorage(Constants.StorageName, Configs.AdoNetConfig);

        #endregion
    })
    .ConfigureLogging(builder => builder.AddConsole())
    .Build();

await host.StartAsync();

Console.WriteLine("\nPress any key to terminate...\n");
Console.ReadLine();

await host.StopAsync();
