namespace OrleanSpaces.Internals;

internal interface ISpaceSubscriberRegistry
{
    Task AddAsync(ISpaceObserver observer);
    Task RemoveAsync(ISpaceObserver observer);
}