using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace OrleanSpaces.Observables;

internal sealed class SpaceSubscribersRegistry
{
    private readonly ILogger<SpaceSubscribersRegistry> logger;
    private readonly SpaceObserverManager manager;

    public SpaceSubscribersRegistry(
        ILogger<SpaceSubscribersRegistry> logger,
        SpaceObserverManager manager)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.manager = manager ?? throw new ArgumentNullException(nameof(manager));
    }

    public Task SubscribeAsync(ISpaceObserver observer)
    {
        if (!manager.IsSubscribed(observer))
        {
            manager.Subscribe(observer);
            logger.LogInformation($"Subscribed observer: {observer.GetType().FullName}");
        }

        return Task.CompletedTask;
    }

    public Task UnsubscribeAsync(ISpaceObserver observer)
    {
        if (manager.IsSubscribed(observer))
        {
            manager.Unsubscribe(observer);
            logger.LogInformation($"Unsubscribed observer: {observer.GetType().FullName}");
        }

        return Task.CompletedTask;
    }
}