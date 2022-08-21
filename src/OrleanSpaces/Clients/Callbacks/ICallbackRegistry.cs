using OrleanSpaces.Core.Primitives;

namespace OrleanSpaces.Clients.Callbacks;

internal interface ICallbackRegistry
{
    void Register(SpaceTemplate template, Func<SpaceTuple, Task> callback);
}