namespace OrleanSpaces.Observers;

public readonly ref struct ObserverRef
{
    public readonly Guid Id;
    public readonly ISpaceObserver Observer;

    public ObserverRef(Guid id, ISpaceObserver observer)
    {
        Id = id;
        Observer = observer;
    }
}