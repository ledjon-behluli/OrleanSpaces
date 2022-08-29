using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrleanSpaces.Callbacks.Continuations;
using OrleanSpaces.Primitives;
using OrleanSpaces.Utils;

namespace OrleanSpaces.Callbacks;

internal class CallbackManager : BackgroundService
{
    private readonly CallbackRegistry registry;
    private readonly ILogger<CallbackManager> logger;
    
    public CallbackManager(
        CallbackRegistry registry,
        ILogger<CallbackManager> logger)
    {
        this.registry = registry ?? throw new ArgumentNullException(nameof(registry));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        logger.LogDebug("Callback manager started.");

        await foreach (var tuple in CallbackChannel.Reader.ReadAllAsync(cancellationToken))
        {
            try
            {
                var entries = registry.Take(tuple);
                await ParallelExecutor.WhenAll(entries, async entry =>
                {
                    if (entry.IsDestructive)
                    {
                        await ContinuationChannel.Writer.WriteAsync(SpaceTemplate.Create(tuple));
                    }

                    await entry.Callback(tuple);
                });


            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
            }
        }

        logger.LogDebug("Callback manager stopped.");
    }
}