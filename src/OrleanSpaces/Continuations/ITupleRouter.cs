using OrleanSpaces.Tuples;

namespace OrleanSpaces.Continuations;

internal interface ITupleRouter
{
    Task RouteAsync(SpaceTuple tuple);
    ValueTask RouteAsync(SpaceTemplate template);
}


internal interface ITupleRouter<T, TTuple, TTemplate>
    where T : unmanaged
    where TTuple : ISpaceTuple<T>
    where TTemplate : ISpaceTemplate<T>
{
    Task RouteAsync(TTuple tuple);
    ValueTask RouteAsync(TTemplate template);
}
