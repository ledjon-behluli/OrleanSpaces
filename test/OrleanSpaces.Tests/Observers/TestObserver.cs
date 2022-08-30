using OrleanSpaces.Observers;
using OrleanSpaces.Primitives;

namespace OrleanSpaces.Tests.Observers;

public class TestObserver : ISpaceObserver
{
    public Task OnTupleAsync(SpaceTuple tuple) => Task.CompletedTask;
}
