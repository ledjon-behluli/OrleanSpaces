using OrleanSpaces.Tuples;

namespace OrleanSpaces.Continuations;

internal interface ITupleRouter<TTuple, TTemplate>
    where TTuple : ISpaceTuple
    where TTemplate : ISpaceTemplate
{
    Task RouteAsync(TTuple tuple);
    ValueTask RouteAsync(TTemplate template);
}