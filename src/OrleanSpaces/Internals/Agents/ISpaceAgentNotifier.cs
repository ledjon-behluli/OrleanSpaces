namespace OrleanSpaces.Internals.Agents;

internal interface ISpaceAgentNotifier
{
    void Broadcast(Action<ISpaceAgent> action);
}
