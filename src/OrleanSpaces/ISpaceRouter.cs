using OrleanSpaces.Tuples;

namespace OrleanSpaces;

internal interface ISpaceRouter<TTuple, TTemplate>
    where TTuple : ISpaceTuple
    where TTemplate : ISpaceTemplate
{
    void RouteStore(ITupleStore<TTuple> store);
    Task RouteTuple(TTuple tuple);
    ValueTask RouteTemplate(TTemplate template);
    ValueTask RouteAction(TupleAction<TTuple> action);
}