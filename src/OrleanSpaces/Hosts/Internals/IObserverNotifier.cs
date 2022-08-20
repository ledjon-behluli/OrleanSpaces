using OrleanSpaces.Core;

namespace OrleanSpaces.Hosts.Internals;

internal interface IObserverNotifier
{
    void Broadcast(Action<ISpaceObserver> action);
}
