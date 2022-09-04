﻿using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrleanSpaces.Primitives;
using OrleanSpaces.Utils;

namespace OrleanSpaces.Observers;

internal class ObserverProcessor : BackgroundService
{
    private readonly ObserverRegistry registry;
    private readonly ObserverChannel channel;
    private readonly ILogger<ObserverProcessor> logger;

    public ObserverProcessor(
        ObserverRegistry registry,
        ObserverChannel channel,
        ILogger<ObserverProcessor> logger)
    {
        this.registry = registry ?? throw new ArgumentNullException(nameof(registry));
        this.channel = channel ?? throw new ArgumentNullException(nameof(channel));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        logger.LogDebug("Observer processor started.");

        await foreach (SpaceTuple tuple in channel.Reader.ReadAllAsync(cancellationToken))
        {
            await ParallelExecutor.WhenAll(registry.Observers, async x =>
            {
                try
                {
                    await x.OnTupleAsync(tuple);
                }
                catch (Exception e)
                {
                    logger.LogError(e, e.Message);
                }
            });
        }

        logger.LogDebug("Observer processor stopped.");
    }
}
