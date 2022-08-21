namespace OrleanSpaces.Core.Observers;

internal interface IObserverRegistry
{
    void Register(ISpaceObserver observer);
    void Deregister(ISpaceObserver observer);
}