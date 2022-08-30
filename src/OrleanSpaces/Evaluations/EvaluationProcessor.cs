using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrleanSpaces.Continuations;
using OrleanSpaces.Primitives;

namespace OrleanSpaces.Evaluations;

internal class EvaluationProcessor : BackgroundService
{
    private readonly ILogger<EvaluationProcessor> logger;

    public EvaluationProcessor(ILogger<EvaluationProcessor> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        logger.LogDebug("Evaluation processor started.");

        await foreach (var evaluator in EvaluationChannel.Reader.ReadAllAsync(cancellationToken))
        {
            try
            {
                SpaceTuple tuple = await evaluator();
                await ContinuationChannel.Writer.WriteAsync(tuple);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
            }
        }

        logger.LogDebug("Evaluation processor stopped.");
    }
}