using Orleans;
using OrleanSpaces;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Hosting;
using Microsoft.Extensions.Hosting;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        var client = new ClientBuilder()
            .UseLocalhostClustering()
            .UseTupleSpace()
                .AddSimpleMessageStreamProvider(StreamNames.PubSubProvider)
            .Build();

        services.AddSingleton(client);
        services.AddHostedService<Worker>();
        services.AddTupleSpace();
    })
    .Build();

await host.RunAsync();