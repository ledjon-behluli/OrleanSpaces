using Microsoft.Extensions.Logging;

namespace OrleanSpaces.Internals;

internal class SubscriberRegistry
{
    private readonly ILogger<SubscriberRegistry> logger;
    private readonly ObserverManager manager;

    public SubscriberRegistry(
        ILogger<SubscriberRegistry> logger,
        ObserverManager manager)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.manager = manager ?? throw new ArgumentNullException(nameof(manager));
    }

    public Task SubscribeAsync(ISpaceObserver observer)
    {
        if (!manager.IsSubscribed(observer))
        {
            manager.Subscribe(observer);
            logger.LogInformation($"Subscribed: '{observer.GetType().FullName}'. Total number of subscribers: {manager.Count}");
        }

        return Task.CompletedTask;
    }

    public Task UnsubscribeAsync(ISpaceObserver observer)
    {
        if (manager.IsSubscribed(observer))
        {
            manager.Unsubscribe(observer);
            logger.LogInformation($"Unsubscribed: '{observer.GetType().FullName}'. Total number of subscribers: {manager.Count}");
        }

        return Task.CompletedTask;
    }
}