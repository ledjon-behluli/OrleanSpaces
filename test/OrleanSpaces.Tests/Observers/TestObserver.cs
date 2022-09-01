using OrleanSpaces.Observers;
using OrleanSpaces.Primitives;

namespace OrleanSpaces.Tests.Observers;

internal class TestObserver : ISpaceObserver
{
    public SpaceTuple LastReceived { get; private set; }

    public virtual Task OnTupleAsync(SpaceTuple tuple)
    {
        LastReceived = tuple;
        return Task.CompletedTask;
    }
}

internal class ThrowingTestObserver : TestObserver
{
    public override Task OnTupleAsync(SpaceTuple tuple)
    {
        throw new Exception("Test");
    }
}