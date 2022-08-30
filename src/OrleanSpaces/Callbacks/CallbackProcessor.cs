using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrleanSpaces.Callbacks.Continuations;
using OrleanSpaces.Primitives;
using OrleanSpaces.Utils;

namespace OrleanSpaces.Callbacks;

internal class CallbackProcessor : BackgroundService
{
    private readonly CallbackRegistry registry;
    private readonly ILogger<CallbackProcessor> logger;
    
    public CallbackProcessor(
        CallbackRegistry registry,
        ILogger<CallbackProcessor> logger)
    {
        this.registry = registry ?? throw new ArgumentNullException(nameof(registry));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        logger.LogDebug("Callback processor started.");

        await foreach (var tuple in CallbackChannel.Reader.ReadAllAsync(cancellationToken))
        {
            try
            {
                var entries = registry.Take(tuple);
                await ParallelExecutor.WhenAll(entries, async entry =>
                {
                    await entry.Callback(tuple);
                    if (entry.IsDestructive)
                    {
                        await ContinuationChannel.Writer.WriteAsync(SpaceTemplate.Create(tuple));
                    }
                });
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
            }
        }

        logger.LogDebug("Callback processor stopped.");
    }
}