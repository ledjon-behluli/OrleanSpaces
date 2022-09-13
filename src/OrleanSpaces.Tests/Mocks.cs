using Microsoft.Extensions.Hosting;
using OrleanSpaces.Continuations;
using OrleanSpaces.Observers;
using OrleanSpaces.Primitives;
using System.Runtime.CompilerServices;

namespace OrleanSpaces.Tests;

public struct TestStruct { }
public class TestClass { }
public enum TestEnum { A }

public class TestTupleRouter : ITupleRouter
{
    public ITuple Tuple { get; private set; }

    public Task RouteAsync(ITuple tuple)
    {
        Tuple = tuple;
        return Task.CompletedTask;
    }
}

public class TestObserver : ISpaceObserver
{
    public SpaceTuple LastReceived { get; private set; } = SpaceTuple.Passive;
    public bool SpaceEmptiedReceived { get; private set; }

    public virtual Task OnNewTupleAsync(SpaceTuple tuple, CancellationToken cancellationToken)
    {
        LastReceived = tuple;
        return Task.CompletedTask;
    }

    public Task OnEmptySpaceAsync(CancellationToken cancellationToken)
    {
        SpaceEmptiedReceived = true;
        return Task.CompletedTask;
    }
}

public class ThrowingTestObserver : TestObserver
{
    public override Task OnNewTupleAsync(SpaceTuple tuple, CancellationToken cancellationToken)
    {
        throw new Exception("Test");
    }
}

public class TestHostAppLifetime : IHostApplicationLifetime
{
    private readonly CancellationTokenSource stoppedSource = new();

    public CancellationToken ApplicationStarted => CancellationToken.None;
    public CancellationToken ApplicationStopping => CancellationToken.None;
    public CancellationToken ApplicationStopped => stoppedSource.Token;

    public void StopApplication() => stoppedSource.Cancel();
}
