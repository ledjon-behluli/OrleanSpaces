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

public class TestObserver : DynamicObserver
{
    public SpaceTuple LastTuple { get; private set; } = new();
    public SpaceTemplate LastTemplate { get; private set; } = new();
    public bool LastFlattening { get; private set; }

    public TestObserver() => Interested(In.Everything);

    public override Task OnExpansionAsync(SpaceTuple tuple, CancellationToken cancellationToken)
    {
        LastTuple = tuple;
        return Task.CompletedTask;
    }

    public override Task OnContractionAsync(SpaceTemplate template, CancellationToken cancellationToken)
    {
        LastTemplate = template;
        return Task.CompletedTask;
    }

    public override Task OnFlatteningAsync(CancellationToken cancellationToken)
    {
        LastFlattening = true;
        return Task.CompletedTask;
    }
}

public class ThrowingTestObserver : TestObserver
{
    public override Task OnExpansionAsync(SpaceTuple tuple, CancellationToken cancellationToken)
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
