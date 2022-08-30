using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrleanSpaces.Bridges;

namespace OrleanSpaces.Callbacks.Continuations;

internal class ContinuationProcessor : BackgroundService
{
    private readonly SpaceGrainBridge bridge;
    private readonly ILogger<ContinuationProcessor> logger;

    public ContinuationProcessor(
        SpaceGrainBridge bridge,
        ILogger<ContinuationProcessor> logger)
    {
        this.bridge = bridge ?? throw new ArgumentNullException(nameof(bridge));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        logger.LogDebug("Callback continuation processor started.");

        await foreach (var template in ContinuationChannel.Reader.ReadAllAsync(cancellationToken))
        {
            try
            {
                _ = await bridge.PopAsync(template);
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
            }
        }

        logger.LogDebug("Callback continuation processor stopped.");
    }
}