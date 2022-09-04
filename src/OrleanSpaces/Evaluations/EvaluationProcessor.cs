using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrleanSpaces.Continuations;
using OrleanSpaces.Primitives;

namespace OrleanSpaces.Evaluations;

internal class EvaluationProcessor : BackgroundService
{
    private readonly EvaluationChannel evaluationChannel;
    private readonly ContinuationChannel continuationChannel;
    private readonly ILogger<EvaluationProcessor> logger;

    public EvaluationProcessor(
        EvaluationChannel evaluationChannel,
        ContinuationChannel continuationChannel,
        ILogger<EvaluationProcessor> logger)
    {
        this.evaluationChannel = evaluationChannel ?? throw new ArgumentNullException(nameof(evaluationChannel));
        this.continuationChannel = continuationChannel ?? throw new ArgumentNullException(nameof(continuationChannel));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        logger.LogDebug("Evaluation processor started.");

        await foreach (var evaluation in evaluationChannel.Reader.ReadAllAsync(cancellationToken))
        {
            try
            {
                SpaceTuple tuple = await evaluation();
                await continuationChannel.Writer.WriteAsync(tuple);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
            }
        }

        logger.LogDebug("Evaluation processor stopped.");
    }
}