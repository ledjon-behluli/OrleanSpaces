using OrleanSpaces.Continuations;
using OrleanSpaces.Primitives;

namespace OrleanSpaces.Tests.Continuations;

internal class TestRouter : ISpaceElementRouter
{
    public ISpaceElement Element { get; private set; }
    
    public Task RouteAsync(ISpaceElement element)
    {
        Element = element;
        return Task.CompletedTask;
    }
}