using Microsoft.Extensions.DependencyInjection;
using Orleans;
using OrleanSpaces.Clients;
using OrleanSpaces.Core.Primitives;

var client = new ClientBuilder()
    .UseLocalhostClustering()
    .UseTupleSpace()
    .Build();

await client.Connect();

Console.WriteLine("Connected to the tuple space.\n\n");

var spaceClient = client.ServiceProvider.GetRequiredService<ISpaceClient>();

Console.WriteLine("\n\nPress any key to terminate...\n\n");
Console.ReadLine();

await client.Close();