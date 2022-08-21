using Microsoft.Extensions.Logging;
using OrleanSpaces.Core.Observers;
using System.Collections.Concurrent;

namespace OrleanSpaces.Hosts.Observers;

internal class ObserverManager : IObserverRegistry, IObserverNotifier
{
    private readonly ILogger<ObserverManager> logger;
    private readonly ConcurrentDictionary<ISpaceObserver, DateTime> observers;

    public ObserverManager(ILogger<ObserverManager> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        observers = new ConcurrentDictionary<ISpaceObserver, DateTime>();
    }

    public void Register(ISpaceObserver observer)
    {
        if (observers.TryAdd(observer, DateTime.UtcNow))
        {
            logger.LogInformation("Observer Registration - Current number of observers: {ObserversCount}", observers.Count);
        }
    }

    public void Deregister(ISpaceObserver observer)
    {
        if (observers.TryRemove(observer, out _))
        {
            logger.LogInformation("Observer Deregistration - Current number of observers: {ObserversCount}", observers.Count);
        }
    }

    public void Broadcast(Action<ISpaceObserver> action)
    {
        List<ISpaceObserver> defected = new();

        foreach (var observer in observers)
        {
            try
            {
                action(observer.Key);
            }
            catch (Exception)
            {
                defected ??= new List<ISpaceObserver>();
                defected.Add(observer.Key);
            }
        }

        if (defected.Count > 0)
        {
            foreach (var observer in defected)
            {
                Deregister(observer);
            }
        }
    }
}
