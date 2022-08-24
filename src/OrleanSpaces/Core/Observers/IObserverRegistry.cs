namespace OrleanSpaces.Core.Observers;

internal interface IObserverRefRegistry
{
    ValueTask RegisterAsync(ISpaceObserver observer);
    ValueTask DeregisterAsync(ISpaceObserver observer);
    ValueTask<bool> IsRegisteredAsync(ISpaceObserver observer);
}