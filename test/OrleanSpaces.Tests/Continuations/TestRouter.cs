using OrleanSpaces.Continuations;
using OrleanSpaces.Primitives;

namespace OrleanSpaces.Tests.Continuations;

internal class TestRouter : ISpaceElementRouter
{
    private readonly List<ISpaceElement> elements = new();
    public IEnumerable<ISpaceElement> Elements => elements;

    public Task RouteAsync(ISpaceElement element)
    {
        elements.Add(element);
        return Task.CompletedTask;
    }
}