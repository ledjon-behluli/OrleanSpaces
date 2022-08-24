using OrleanSpaces.Core.Observers;
namespace OrleanSpaces.Clients.Observers;

public interface ISpaceObserverRef
{
    ISpaceObserver Observer { get; }
}

internal class SpaceObserverRef : ISpaceObserverRef
{
    public ISpaceObserver Observer { get; }

    public SpaceObserverRef(ISpaceObserver observer)
    {
        Observer = observer;
    }
}