using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrleanSpaces.Primitives;
using OrleanSpaces.Utils;

namespace OrleanSpaces.Observers;

internal class ObserverManager : BackgroundService
{
    private readonly ObserverRegistry registry;
    private readonly ILogger<ObserverManager> logger;
    public ObserverManager(
        ObserverRegistry registry,
        ILogger<ObserverManager> logger)
    {
        this.registry = registry ?? throw new ArgumentNullException(nameof(registry));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        logger.LogDebug("Observer manager started.");

        await foreach (SpaceTuple tuple in ObserverChannel.Reader.ReadAllAsync(cancellationToken))
        {
            try
            {
                await ParallelExecutor.WhenAll(registry.Observers, x => x.OnTupleAsync(tuple));
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
            }
        }

        logger.LogDebug("Observer manager stopped.");
    }
}
