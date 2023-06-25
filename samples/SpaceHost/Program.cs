using OrleanSpaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;

//https://learn.microsoft.com/en-us/dotnet/orleans/migration-guide
//https://learn.microsoft.com/en-us/dotnet/orleans/host/configuration-guide/local-development-configuration?pivots=orleans-7-0
//https://github.com/dotnet/samples/tree/main/orleans/ChatRoom

var host = Host.CreateDefaultBuilder(args)
    .UseOrleans(siloBuilder =>
    {
        siloBuilder
             .UseLocalhostClustering()
        #region Streaming

            .AddMemoryStreams(Constants.PubSubProvider)
            //.AddAzureQueueStreams(Constants.PubSubProvider, Configs.QueueConfig)

        #endregion

        #region Persistence

            .AddMemoryGrainStorage(Constants.PubSubStore)
            .AddMemoryGrainStorage(Constants.TupleSpacesStore);
        //.AddMemoryGrainStorage(Constants.SpaceGrainStorage)
        //.AddMemoryGrainStorage(Constants.IntGrainStorage)

        //.AddAzureTableGrainStorage(Constants.PubSubStore, Configs.TableConfig)
        //.AddAzureTableGrainStorage(Constants.TupleSpacesStore, Configs.TableConfig);

        //.AddAzureBlobGrainStorage(Constants.PubSubStore, Configs.BlobConfig)
        //.AddAzureBlobGrainStorage(Constants.TupleSpacesStore, Configs.BlobConfig);

        //.AddAdoNetGrainStorage(Constants.PubSubStore, Configs.AdoNetConfig)
        //.AddAdoNetGrainStorage(Constants.TupleSpacesStore, Configs.AdoNetConfig);

        #endregion

    })
    .ConfigureLogging(builder => builder.AddConsole())
    .Build();

await host.StartAsync();

Console.WriteLine("\nPress any key to terminate...\n");
Console.ReadLine();

await host.StopAsync();
