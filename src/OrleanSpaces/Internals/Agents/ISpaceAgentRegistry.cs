namespace OrleanSpaces.Internals.Agents;

internal interface ISpaceAgentRegistry
{
    void Register(ISpaceAgent agent);
    void Deregister(ISpaceAgent agent);
}