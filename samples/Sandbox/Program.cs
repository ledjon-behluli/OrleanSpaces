using Orleans;
using OrleanSpaces;
using Orleans.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

await Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddTupleSpace();
        services.AddHostedService<Worker>();
    })
    .RunConsoleAsync();
