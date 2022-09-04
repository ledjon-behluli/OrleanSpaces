using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrleanSpaces.Primitives;

namespace OrleanSpaces.Continuations;

internal class ContinuationProcessor : BackgroundService
{
    private readonly ISpaceChannel spaceChannel;
    private readonly ContinuationChannel continuationChannel;
    private readonly ILogger<ContinuationProcessor> logger;

    public ContinuationProcessor(
        ISpaceChannel spaceChannel,
        ContinuationChannel continuationChannel,
        ILogger<ContinuationProcessor> logger)
    {
        this.spaceChannel = spaceChannel ?? throw new ArgumentNullException(nameof(spaceChannel));
        this.continuationChannel = continuationChannel ?? throw new ArgumentNullException(nameof(continuationChannel));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        logger.LogDebug("Callback continuation processor started.");
 
        await foreach (var element in continuationChannel.Reader.ReadAllAsync(cancellationToken))
        {
            var agent = await spaceChannel.GetAsync();
            Type type = element.GetType();

            if (type == typeof(SpaceTuple))
            {
                await agent.WriteAsync((SpaceTuple)element);
            }
            else if (type == typeof(SpaceTemplate))
            {
                _ = await agent.PopAsync((SpaceTemplate)element);
            }
            else
            {
                throw new NotImplementedException($"No implementation exists for '{type}'");
            }
        }

        logger.LogDebug("Callback continuation processor stopped.");
    }
}