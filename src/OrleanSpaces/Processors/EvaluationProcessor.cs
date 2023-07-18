using Microsoft.Extensions.Hosting;
using OrleanSpaces.Channels;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Processors;

internal sealed class EvaluationProcessor<TTuple, TTemplate> : BackgroundService
    where TTuple : ISpaceTuple
    where TTemplate : ISpaceTemplate
{
    private readonly SpaceOptions options;
    private readonly EvaluationChannel<TTuple> evaluationChannel;
    private readonly ContinuationChannel<TTuple, TTemplate> continuationChannel;

    public EvaluationProcessor(
        SpaceOptions options,
        EvaluationChannel<TTuple> evaluationChannel,
        ContinuationChannel<TTuple, TTemplate> continuationChannel)
    {
        this.options = options;
        this.evaluationChannel = evaluationChannel ?? throw new ArgumentNullException(nameof(evaluationChannel));
        this.continuationChannel = continuationChannel ?? throw new ArgumentNullException(nameof(continuationChannel));
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await foreach (Func<Task<TTuple>> evaluation in evaluationChannel.Reader.ReadAllAsync(cancellationToken))
        {
            try
            {
                TTuple tuple = await evaluation();
                await continuationChannel.TupleWriter.WriteAsync(tuple, cancellationToken);
            }
            catch
            {
                if (!options.HandleEvaluationExceptions)
                {
                    throw;
                }
            }
        }
    }
}