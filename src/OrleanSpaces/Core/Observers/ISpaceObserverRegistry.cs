namespace OrleanSpaces.Core.Observers;

internal interface ISpaceObserverRegistry
{
    ValueTask<bool> IsRegisteredAsync(ISpaceObserver observer);
    ValueTask RegisterAsync(ISpaceObserver observer);
    ValueTask DeregisterAsync(ISpaceObserver observer);
}