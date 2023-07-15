using OrleanSpaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using OrleanSpaces.Tuples;
using OrleanSpaces.Tuples.Specialized;

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

ISpaceTuple tuple = new SpaceTuple();
ISpaceTemplate template = new SpaceTemplate(1);

ISpaceTuple<int> tuple1 = new IntTuple();
ISpaceTemplate<int> template1 = new IntTemplate(1);

Console.WriteLine("\nPress any key to terminate...\n");
Console.ReadKey();

await host.StopAsync();

class Test : ISpaceTuple
{
    public int Length => throw new NotImplementedException();
}

class TestInt : ISpaceTuple<int>
{
    public ref readonly int this[int index] => throw new NotImplementedException();

    public int Length => throw new NotImplementedException();

    public ReadOnlySpan<char> AsSpan()
    {
        throw new NotImplementedException();
    }

    public ReadOnlySpan<int>.Enumerator GetEnumerator()
    {
        throw new NotImplementedException();
    }
}
