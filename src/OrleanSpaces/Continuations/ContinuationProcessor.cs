using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace OrleanSpaces.Continuations;

internal class ContinuationProcessor : BackgroundService
{
    private readonly ContinuationChannel channel;
    private readonly ISpaceElementRouter router;
    private readonly ILogger<ContinuationProcessor> logger;

    public ContinuationProcessor(
        ContinuationChannel channel,
        ISpaceElementRouter router,
        ILogger<ContinuationProcessor> logger)
    {
        this.channel = channel ?? throw new ArgumentNullException(nameof(channel));
        this.router = router ?? throw new ArgumentNullException(nameof(router));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        logger.LogDebug("Callback continuation processor started.");
 
        await foreach (var element in channel.Reader.ReadAllAsync(cancellationToken))
        {
            await router.RouteAsync(element);
        }

        logger.LogDebug("Callback continuation processor stopped.");
    }
}