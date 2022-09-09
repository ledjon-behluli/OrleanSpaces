using Microsoft.Extensions.Hosting;
using OrleanSpaces.Continuations;
using OrleanSpaces.Primitives;

namespace OrleanSpaces.Evaluations;

internal class EvaluationProcessor : BackgroundService
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
        await foreach (var evaluation in evaluationChannel.Reader.ReadAllAsync(cancellationToken))
        {
            try
            {
                SpaceTuple tuple = await evaluation();
                await continuationChannel.Writer.WriteAsync(tuple, cancellationToken);
            }
            catch (Exception)
            {
                lifetime.StopApplication();
            }
        }
    }
}