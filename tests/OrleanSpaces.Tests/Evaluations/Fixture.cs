using OrleanSpaces.Continuations;
using OrleanSpaces.Evaluations;

namespace OrleanSpaces.Tests.Evaluations;

public class Fixture : IAsyncLifetime
{
    private readonly EvaluationProcessor processor;

    internal EvaluationChannel EvaluationChannel { get; }
    internal ContinuationChannel ContinuationChannel { get; }

    public Fixture()
    {
        EvaluationChannel = new();
        ContinuationChannel = new();
        processor = new(new TestHostAppLifetime(), EvaluationChannel, ContinuationChannel);
    }

    public Task InitializeAsync() => processor.StartAsync(default);
    public Task DisposeAsync() => processor.StopAsync(default);
}