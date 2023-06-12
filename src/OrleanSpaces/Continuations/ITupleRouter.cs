using OrleanSpaces.Tuples;

namespace OrleanSpaces.Continuations;

internal interface ITupleRouter
{
    Task RouteAsync(ISpaceElement element);
}