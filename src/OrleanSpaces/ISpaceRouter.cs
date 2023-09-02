using OrleanSpaces.Tuples;

namespace OrleanSpaces;

internal interface ISpaceRouter<TTuple, TTemplate>
    where TTuple : ISpaceTuple
    where TTemplate : ISpaceTemplate
{
    void RouteDirector(IStoreDirector<TTuple> director);
    Task RouteTuple(TTuple tuple);
    ValueTask RouteTemplate(TTemplate template);
    ValueTask RouteAction(TupleAction<TTuple> action);
}