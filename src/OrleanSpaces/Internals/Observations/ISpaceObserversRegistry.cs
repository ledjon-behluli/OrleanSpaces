namespace OrleanSpaces.Internals.Observations;

internal interface ISpaceObserversRegistry
{
    void Register(ISpaceObserver observer);
    void Deregister(ISpaceObserver observer);
}