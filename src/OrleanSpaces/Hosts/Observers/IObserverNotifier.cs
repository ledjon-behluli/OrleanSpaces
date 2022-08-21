using OrleanSpaces.Core.Observers;

namespace OrleanSpaces.Hosts.Observers;

internal interface IObserverNotifier
{
    void Broadcast(Action<ISpaceObserver> action);
}
