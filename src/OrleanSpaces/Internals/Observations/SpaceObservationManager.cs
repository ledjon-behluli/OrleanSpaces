using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace OrleanSpaces.Internals.Observations;

internal class SpaceObservationManager : ISpaceFluctuationNotifier, ISpaceObserverRegistry
{
    private readonly ILogger<SpaceObservationManager> logger;
    private readonly ConcurrentDictionary<ISpaceObserver, DateTime> observers;

    public SpaceObservationManager(ILogger<SpaceObservationManager> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        observers = new ConcurrentDictionary<ISpaceObserver, DateTime>();
    }

    public void Register(ISpaceObserver observer)
    {
        if (observers.TryAdd(observer, DateTime.UtcNow))
        {
            logger.LogInformation($"Subscribed: '{observer.GetType().FullName}'. Total number of subscribers: {observers.Count}");
        }
    }

    public void Deregister(ISpaceObserver observer)
    {
        if (observers.TryRemove(observer, out _))
        {
            logger.LogInformation($"Unsubscribed: '{observer.GetType().FullName}'. Total number of subscribers: {observers.Count}");
        }
    }

    public void Broadcast(Action<ISpaceObserver> action)
    {
        var defunct = default(List<ISpaceObserver>);

        foreach (var observer in observers)
        {
            try
            {
                action(observer.Key);
            }
            catch (Exception)
            {
                defunct ??= new List<ISpaceObserver>();
                defunct.Add(observer.Key);
            }
        }

        if (defunct != default(List<ISpaceObserver>))
        {
            foreach (var observer in defunct)
            {
                observers.Remove(observer, out _);
            }
        }
    }
}
