namespace OrleanSpaces.Internals.Observations;

internal interface ISpaceFluctuationNotifier
{
    void Broadcast(Action<ISpaceObserver> action);
}
