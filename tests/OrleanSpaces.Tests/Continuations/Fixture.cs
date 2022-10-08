using OrleanSpaces.Continuations;

namespace OrleanSpaces.Tests.Continuations;

public class Fixture : IAsyncLifetime
{
    private readonly ContinuationProcessor processor;

    internal TestTupleRouter Router { get; }
    internal ContinuationChannel Channel { get; }

    public Fixture()
    {
        Channel = new();
        Router = new TestTupleRouter();

        processor = new(Channel, Router);
    }

    public async Task InitializeAsync() => await processor.StartAsync(default);
    public async Task DisposeAsync() => await processor.StopAsync(default);
}
