namespace OrleanSpaces.Observers;

internal class SpaceObserverRef : ISpaceObserverRef
{
    public ISpaceObserver Observer { get; }

    public SpaceObserverRef(ISpaceObserver observer)
    {
        Observer = observer;
    }
}