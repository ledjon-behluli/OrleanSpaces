using OrleanSpaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using OrleanSpaces.Tuples;
using OrleanSpaces.Tuples.Specialized;
using System.Runtime.CompilerServices;

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

// test anything here...

OrleanSpaces.Tuples.ISpaceTuple tuple = new SpaceTuple();
ISpaceTemplate template = new SpaceTemplate(1);

ISpaceTuple<int> tuple1 = new IntTuple();
ISpaceTemplate<int> template1 = new IntTemplate(1);

Console.WriteLine("\nPress any key to terminate...\n");
Console.ReadKey();

await host.StopAsync();

class TestTuple : ISpaceTuple
{
    public int Length => 0;
}

class TestTe : ISpaceTemplate
{
    public int Length => 0;
}

class TestInt : ISpaceTemplate<int>
{
    private int? value;
    public ref readonly int? this[int index] => ref value;
    public int Length => 0;
    public ReadOnlySpan<int?>.Enumerator GetEnumerator() => new();
}

class A { public void B(ISpaceTuple tuple) { } }