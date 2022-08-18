namespace OrleanSpaces.Internals.Observations;

internal interface ISpaceFluctuationsNotifier
{
    void Broadcast(Action<ISpaceObserver> action);
}
