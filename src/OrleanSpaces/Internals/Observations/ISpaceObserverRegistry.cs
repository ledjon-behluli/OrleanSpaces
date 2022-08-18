namespace OrleanSpaces.Internals.Observations;

internal interface ISpaceObserverRegistry
{
    void Register(ISpaceObserver observer);
    void Deregister(ISpaceObserver observer);
}