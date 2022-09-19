using Orleans.Hosting;
using Microsoft.Extensions.Logging;
using OrleanSpaces;
using Microsoft.Extensions.Hosting;
using Orleans;

var host = new SiloHostBuilder()
    .UseLocalhostClustering()
    .AddSimpleMessageStreamProvider(Constants.PubSubProvider)
    .AddMemoryGrainStorage(Constants.PubSubStore)

    // Choose what storage mechanism you want to use for storing the tuple space state.

    .AddMemoryGrainStorage(Constants.TupleSpaceStore)

    //.AddAzureTableGrainStorage(Constants.TupleSpaceStore, options =>
    //{
    //    options.UseJson = true;
    //    options.ConfigureTableServiceClient("UseDevelopmentStorage=true");
    //})

    //.AddAzureBlobGrainStorage(Constants.TupleSpaceStore, options =>
    //{
    //    options.UseJson = true;
    //    options.ConfigureBlobServiceClient("UseDevelopmentStorage=true");
    //})

    // More infos on ADO.NET: https://dotnet.github.io/orleans/docs/host/configuration_guide/adonet_configuration.html
    //.AddAdoNetGrainStorage(Constants.TupleSpaceStore, options =>
    //{
    //    options.UseJsonFormat = true;
    //    options.Invariant = "Npgsql";
    //    options.ConnectionString = "Host=localhost;Database=OrleanSpaces;Username=postgres;Password=postgres";
    //})

    .AddTupleSpace()
    .ConfigureLogging(builder => builder.AddConsole())
    .Build();

await host.StartAsync();

Console.WriteLine("\nPress any key to terminate...\n");
Console.ReadLine();

await host.StopAsync();