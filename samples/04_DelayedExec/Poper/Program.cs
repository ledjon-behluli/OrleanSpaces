using OrleanSpaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using OrleanSpaces.Tuples;

var host = new HostBuilder()
    .ConfigureAppConfiguration(config => config.AddJsonFile("appsettings.json"))
    .UseOrleansClient(builder =>
    {
        builder.AddOrleanSpaces();
        builder.UseLocalhostClustering();
        builder.AddMemoryStreams(Constants.PubSubProvider);
    })
    .ConfigureLogging((context, builder) =>
    {
        builder.ClearProviders();
        builder.AddConfiguration(context.Configuration.GetSection("Logging"));
        builder.AddConsole();
    })
    .Build();

await host.StartAsync();

Console.WriteLine("Connected to the tuple space.\n\n");

ISpaceAgent agent = host.Services.GetRequiredService<ISpaceAgent>();

const string EXCHANGE_KEY = "sensor-data";
bool callbackExecuted = false;

SpaceTemplate template = new(EXCHANGE_KEY, typeof(double));
Console.WriteLine($"WORKER: Pop-ing a tuple that matches template {template} in a callback fashion...");

await agent.PopAsync(template, async tuple =>
{
    Console.WriteLine($"CALLBACK: Got back response for template {template} in form of this tuple '{tuple}'. Doing some heavy work...");

    await Task.Delay(1000);

    Console.WriteLine("CALLBACK: Done with my work.");
    callbackExecuted = true;
});

Console.WriteLine($"WORKER: Simulating some delay until a tuple that matches template {template} is written...");
await Task.Delay(5000);

SpaceTuple tuple = new(EXCHANGE_KEY, 1.2334);
Console.WriteLine($"WORKER: Writing sensor data in form of the tuple {tuple}");
await agent.WriteAsync(tuple);

while (!callbackExecuted)
{
    Console.WriteLine("WORKER: Doing some other stuff.");
    await Task.Delay(100);
}

Console.WriteLine($"WORKER: Checking if {tuple} is still in space: {!(await agent.Peek(template)).IsEmpty}");


Console.WriteLine("\nPress any key to terminate...");
Console.ReadKey();

await host.StopAsync();