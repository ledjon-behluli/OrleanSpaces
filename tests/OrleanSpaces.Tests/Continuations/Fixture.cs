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

    public Task InitializeAsync() => processor.StartAsync(default);
    public Task DisposeAsync() => processor.StopAsync(default);
}
