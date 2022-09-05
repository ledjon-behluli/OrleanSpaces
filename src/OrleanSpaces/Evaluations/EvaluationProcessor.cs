using Microsoft.Extensions.Hosting;
using OrleanSpaces.Continuations;
using OrleanSpaces.Primitives;

namespace OrleanSpaces.Evaluations;

internal class EvaluationProcessor : BackgroundService
{
    private readonly EvaluationChannel evaluationChannel;
    private readonly ContinuationChannel continuationChannel;

    public EvaluationProcessor(
        EvaluationChannel evaluationChannel,
        ContinuationChannel continuationChannel)
    {
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
                await continuationChannel.Writer.WriteAsync(tuple);
            }
            catch (Exception)
            {

            }
        }
    }
}