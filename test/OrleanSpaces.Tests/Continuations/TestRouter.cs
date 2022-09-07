using OrleanSpaces.Continuations;
using OrleanSpaces.Primitives;

namespace OrleanSpaces.Tests.Continuations;

internal class TestRouter : ISpaceTupleRouter
{
    public ISpaceTuple SpaceTuple { get; private set; }
    
    public Task RouteAsync(ISpaceTuple spaceTuple)
    {
        SpaceTuple = spaceTuple;
        return Task.CompletedTask;
    }
}