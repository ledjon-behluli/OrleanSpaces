using OrleanSpaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .UseOrleans(siloBuilder =>
    {
        siloBuilder.UseLocalhostClustering();
        #region Streaming

        siloBuilder.AddMemoryStreams(Constants.PubSubProvider);
        //siloBuilder.AddAzureQueueStreams(Constants.PubSubProvider, Configs.QueueConfig);

        #endregion
        #region Persistence

        siloBuilder.AddMemoryGrainStorage(Constants.PubSubStore);
        siloBuilder.AddMemoryGrainStorage(Constants.Store_StorageName);

        //siloBuilder.AddAzureTableGrainStorage(Constants.PubSubStore, Configs.TableConfig);
        //siloBuilder.AddAzureTableGrainStorage(Constants.StorageName, Configs.TableConfig);

        //siloBuilder.AddAzureBlobGrainStorage(Constants.PubSubStore, Configs.BlobConfig);
        //siloBuilder.AddAzureBlobGrainStorage(Constants.StorageName, Configs.BlobConfig);

        //siloBuilder.AddAdoNetGrainStorage(Constants.PubSubStore, Configs.AdoNetConfig);
        //siloBuilder.AddAdoNetGrainStorage(Constants.StorageName, Configs.AdoNetConfig);

        #endregion

    })
    .ConfigureLogging(builder => builder.AddConsole())
    .Build();

await host.StartAsync();

Console.WriteLine("\nPress any key to terminate...\n");
Console.ReadLine();

await host.StopAsync();
