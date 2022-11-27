using OrleanSpaces.Callbacks;
using OrleanSpaces.Continuations;

namespace OrleanSpaces.Tests.Callbacks;

public class Fixture : IAsyncLifetime
{
    private readonly CallbackProcessor processor;

    internal CallbackChannel CallbackChannel { get; }
    internal ContinuationChannel ContinuationChannel { get; }
    internal CallbackRegistry Registry { get; private set; }

    public Fixture()
    {
        CallbackChannel = new();
        ContinuationChannel = new();
        Registry = new();
        processor = new(new TestHostAppLifetime(), Registry, CallbackChannel, ContinuationChannel);
    }

    public Task InitializeAsync() => processor.StartAsync(default);
    public Task DisposeAsync() => processor.StopAsync(default);
}
