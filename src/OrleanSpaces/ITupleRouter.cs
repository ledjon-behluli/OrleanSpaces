using OrleanSpaces.Tuples;

namespace OrleanSpaces;

internal interface ITupleRouter<TTuple, TTemplate>
    where TTuple : ISpaceTuple
    where TTemplate : ISpaceTemplate
{
    Task RouteAsync(TTuple tuple);
    ValueTask RouteAsync(TTemplate template);
}