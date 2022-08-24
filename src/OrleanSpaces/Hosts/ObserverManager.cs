using OrleanSpaces.Core.Observers;
using System.Collections.Concurrent;

namespace OrleanSpaces.Hosts;

internal class ObserverManager
{
    private readonly ConcurrentDictionary<ISpaceObserver, DateTime> observers = new();

    public int Count => observers.Count;
    public IEnumerable<ISpaceObserver> Observers => observers.Keys;

    public bool TryAdd(ISpaceObserver observer) =>
        observers.TryAdd(observer, DateTime.UtcNow);

    public bool TryRemove(ISpaceObserver observer) =>
        observers.TryRemove(observer, out _);

    public void Broadcast(Action<ISpaceObserver> notification)
    {
        List<ISpaceObserver> defected = new();

        foreach (var observer in observers)
        {
            try
            {
                notification(observer.Key);
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
                TryRemove(observer);
            }
        }
    }
}