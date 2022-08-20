using OrleanSpaces.Core.Observers;

namespace OrleanSpaces.Hosts.Internals;

internal interface IObserverNotifier
{
    void Broadcast(Action<ISpaceObserver> action);
}
