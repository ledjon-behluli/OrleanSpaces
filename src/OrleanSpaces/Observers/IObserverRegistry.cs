namespace OrleanSpaces.Observers;

internal interface IObserverRegistry
{
    Guid Register(ISpaceObserver observer);
    void Deregister(ISpaceObserver observer);
}