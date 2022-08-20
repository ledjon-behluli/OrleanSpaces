using OrleanSpaces.Core;
using OrleanSpaces.Core.Primitives;

namespace OrleanSpaces.Clients.Callbacks;

internal interface ICallbackBuffer
{
    void Buffer(SpaceTemplate template, Func<SpaceTuple, Task> callback);
}