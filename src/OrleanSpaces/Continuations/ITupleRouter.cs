using OrleanSpaces.Tuples;

namespace OrleanSpaces.Continuations;

internal interface ITupleRouter
{
    Task RouteAsync(ISpaceTuple tuple);
    ValueTask RouteAsync(ISpaceTemplate template);
}