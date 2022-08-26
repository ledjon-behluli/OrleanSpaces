using Orleans;
using OrleanSpaces;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Hosting;
using Microsoft.Extensions.Hosting;

await Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddTupleSpace();
        services.AddHostedService<ConsoleClient>();
    })
    .Build()
    .RunAsync();