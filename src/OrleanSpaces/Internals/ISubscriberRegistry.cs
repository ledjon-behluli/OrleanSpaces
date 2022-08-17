namespace OrleanSpaces.Internals;

internal interface ISubscriberRegistry
{
    Task AddAsync(ISpaceObserver observer);
    Task RemoveAsync(ISpaceObserver observer);
}