using Microsoft.Extensions.Hosting;
using OrleanSpaces.Continuations;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Evaluations;

internal sealed class EvaluationProcessor<TTuple, TTemplate> : BackgroundService
    where TTuple : ISpaceTuple
    where TTemplate : ISpaceTemplate
{
    private readonly EvaluationChannel<TTuple> evaluationChannel;
    private readonly ContinuationChannel<TTuple, TTemplate> continuationChannel;

    public EvaluationProcessor(
        EvaluationChannel<TTuple> evaluationChannel,
        ContinuationChannel<TTuple, TTemplate> continuationChannel)
    {
        this.evaluationChannel = evaluationChannel ?? throw new ArgumentNullException(nameof(evaluationChannel));
        this.continuationChannel = continuationChannel ?? throw new ArgumentNullException(nameof(continuationChannel));
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await foreach (Func<Task<TTuple>> evaluation in evaluationChannel.Reader.ReadAllAsync(cancellationToken))
        {
            TTuple tuple = await evaluation();
            await continuationChannel.TupleWriter.WriteAsync(tuple, cancellationToken);
        }
    }
}