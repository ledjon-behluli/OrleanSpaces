using OrleanSpaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

var host = new HostBuilder()
    .UseOrleansClient(builder =>
    {
        builder.AddOrleanSpaces();
        builder.UseLocalhostClustering();
        builder.AddMemoryStreams(Constants.PubSubProvider);
    })
    .Build();

var client = host.Services.GetRequiredService<IClusterClient>();
await host.StartAsync();

Console.WriteLine("Connected to the tuple space.\n\n");

var agent = host.Services.GetService<ISpaceAgent>();

var a = await agent.ScanAsync(new(1, 1));


// test anything here...

Console.WriteLine("\nPress any key to terminate...\n");
Console.ReadKey();

await host.StopAsync();