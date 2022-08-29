using OrleanSpaces.Primitives;

namespace OrleanSpaces.Callbacks;

internal interface ICallbackRegistry
{
    void Register(SpaceTemplate template, CallbackEntry entry);
}