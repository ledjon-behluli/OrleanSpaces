using OrleanSpaces.Primitives;

namespace OrleanSpaces.Continuations;

internal interface ISpaceTupleRouter
{
    Task RouteAsync(ISpaceTuple spaceTuple);
}
