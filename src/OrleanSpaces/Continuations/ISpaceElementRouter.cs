using OrleanSpaces.Primitives;

namespace OrleanSpaces.Continuations;

internal interface ISpaceElementRouter
{
    Task RouteAsync(ISpaceElement spaceElement);
}
