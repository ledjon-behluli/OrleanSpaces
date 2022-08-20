namespace OrleanSpaces.Core.Internals;

internal interface IObserverRegistry
{
    void Register(ISpaceObserver observer);
    void Deregister(ISpaceObserver observer);
}