using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrleanSpaces.Bridges;
using OrleanSpaces.Primitives;

namespace OrleanSpaces.Continuations;

internal class ContinuationProcessor : BackgroundService
{
    private readonly SpaceAgent bridge;
    private readonly ILogger<ContinuationProcessor> logger;

    public ContinuationProcessor(
        SpaceAgent bridge,
        ILogger<ContinuationProcessor> logger)
    {
        this.bridge = bridge ?? throw new ArgumentNullException(nameof(bridge));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        logger.LogDebug("Callback continuation processor started.");

        await foreach (var element in ContinuationChannel.Reader.ReadAllAsync(cancellationToken))
        {
            try
            {
                if (element.GetType() == typeof(SpaceTuple))
                {
                    await bridge.WriteAsync((SpaceTuple)element);
                    continue;
                }

                if (element.GetType() == typeof(SpaceTemplate))
                {
                    _ = await bridge.PopAsync((SpaceTemplate)element);
                    continue;
                }

                throw new InvalidOperationException();
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
            }
        }

        logger.LogDebug("Callback continuation processor stopped.");
    }
}
