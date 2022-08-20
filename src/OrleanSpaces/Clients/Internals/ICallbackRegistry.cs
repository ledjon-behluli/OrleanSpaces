using OrleanSpaces.Core;

namespace OrleanSpaces.Clients.Internals;

internal interface ICallbackRegistry
{
    void Register(SpaceTemplate template, Func<SpaceTuple, Task> callback);
}