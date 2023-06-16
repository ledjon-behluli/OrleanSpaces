using Microsoft.Extensions.Hosting;
using OrleanSpaces.Continuations;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Evaluations;

internal sealed class EvaluationProcessor : BackgroundService
{
    private readonly IHostApplicationLifetime lifetime;
    private readonly EvaluationChannel evaluationChannel;
    private readonly ContinuationChannel continuationChannel;

    public EvaluationProcessor(
        IHostApplicationLifetime lifetime,
        EvaluationChannel evaluationChannel,
        ContinuationChannel continuationChannel)
    {
        this.lifetime = lifetime ?? throw new ArgumentNullException(nameof(lifetime));
        this.evaluationChannel = evaluationChannel ?? throw new ArgumentNullException(nameof(evaluationChannel));
        this.continuationChannel = continuationChannel ?? throw new ArgumentNullException(nameof(continuationChannel));
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        evaluationChannel.IsBeingConsumed = true;

        await foreach (Func<Task<SpaceTuple>> evaluation in evaluationChannel.Reader.ReadAllAsync(cancellationToken))
        {
            try
            {
                SpaceTuple tuple = await evaluation();
                await continuationChannel.TupleWriter.WriteAsync(tuple, cancellationToken);
            }
            catch
            {
                lifetime.StopApplication();
            }
        }

        evaluationChannel.IsBeingConsumed = false;
    }
}