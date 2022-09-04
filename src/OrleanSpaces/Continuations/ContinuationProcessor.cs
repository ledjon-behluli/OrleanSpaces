using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrleanSpaces.Primitives;

namespace OrleanSpaces.Continuations;

internal class ContinuationProcessor : BackgroundService
{
    private readonly ISpaceChannel spaceChannel;
    private readonly ContinuationChannel continuation;
    private readonly ILogger<ContinuationProcessor> logger;

    public ContinuationProcessor(
        ISpaceChannel channel,
        ContinuationChannel continuation,
        ILogger<ContinuationProcessor> logger)
    {
        this.spaceChannel = channel ?? throw new ArgumentNullException(nameof(channel));
        this.continuation = continuation ?? throw new ArgumentNullException(nameof(continuation));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        logger.LogDebug("Callback continuation processor started.");
 
        await foreach (var element in continuation.Reader.ReadAllAsync(cancellationToken))
        {
            var agent = await spaceChannel.GetAsync();

            if (element.GetType() == typeof(SpaceTuple))
            {
                await agent.WriteAsync((SpaceTuple)element);
            }

            if (element.GetType() == typeof(SpaceTemplate))
            {
                _ = await agent.PopAsync((SpaceTemplate)element);
            }
        }

        logger.LogDebug("Callback continuation processor stopped.");
    }
}