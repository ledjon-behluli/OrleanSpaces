using OrleanSpaces.Core.Observers;

namespace OrleanSpaces.Clients.Observers;

public interface ISpaceObserverRef
{
    ISpaceObserver Observer { get; }
}