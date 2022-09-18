using Orleans.Hosting;
using Microsoft.Extensions.Logging;
using OrleanSpaces;
using Microsoft.Extensions.Hosting;
using Orleans;

var host = new SiloHostBuilder()
    .UseLocalhostClustering()
    .AddSimpleMessageStreamProvider(Constants.PubSubProvider)
    .AddMemoryGrainStorage(Constants.PubSubStore)
    .AddAzureBlobGrainStorage(name: Constants.TupleSpaceStore, configureOptions: o =>
    {
        o.UseJson = true;
        //o.ContainerName = Constants.TupleSpaceStore;
        o.ConfigureBlobServiceClient("UseDevelopmentStorage=true");
    })
    .AddTupleSpace()
    .ConfigureLogging(builder => builder.AddConsole())
    .Build();

await host.StartAsync();

Console.WriteLine("\nPress any key to terminate...\n");
Console.ReadLine();

await host.StopAsync();