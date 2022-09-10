using Microsoft.Extensions.Hosting;
using OrleanSpaces.Continuations;
using OrleanSpaces.Evaluations;

namespace OrleanSpaces.Tests.Evaluations;

public class Fixture : IAsyncLifetime
{
    private readonly EvaluationProcessor processor;

    internal IHostApplicationLifetime Lifetime;
    internal EvaluationChannel EvaluationChannel { get; }
    internal ContinuationChannel ContinuationChannel { get; }

    public Fixture()
    {
        Lifetime = new TestHostAppLifetime();
        EvaluationChannel = new();
        ContinuationChannel = new();
        processor = new(Lifetime, EvaluationChannel, ContinuationChannel);
    }

    public async Task InitializeAsync() => await processor.StartAsync(default);
    public async Task DisposeAsync() => await processor.StopAsync(default);
}