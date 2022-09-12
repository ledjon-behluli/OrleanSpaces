using Microsoft.Extensions.Hosting;
using OrleanSpaces.Callbacks;
using OrleanSpaces.Continuations;

namespace OrleanSpaces.Tests.Callbacks;

public class Fixture : IAsyncLifetime
{
    private readonly CallbackProcessor processor;

    internal IHostApplicationLifetime Lifetime;
    internal CallbackChannel CallbackChannel { get; }
    internal ContinuationChannel ContinuationChannel { get; }
    internal CallbackRegistry Registry { get; }

    public Fixture()
    {
        Lifetime = new TestHostAppLifetime();
        CallbackChannel = new();
        ContinuationChannel = new();
        Registry = new();
        processor = new(Lifetime, Registry, CallbackChannel, ContinuationChannel);
    }

    public async Task InitializeAsync() => await processor.StartAsync(default);
    public async Task DisposeAsync() => await processor.StopAsync(default);
}
