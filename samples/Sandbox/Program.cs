using OrleanSpaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

await Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddTupleSpace();
        services.AddHostedService<Worker>();
    })
    .UseOrleansClient(builder =>
    {
        builder.UseLocalhostClustering();
        builder.AddMemoryStreams(Constants.PubSubProvider);
    })
    .RunConsoleAsync();