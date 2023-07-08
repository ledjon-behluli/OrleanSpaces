using Microsoft.Extensions.Hosting;
using OrleanSpaces.Continuations;
using OrleanSpaces.Observers;
using OrleanSpaces.Tuples;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Tests;

public struct TestStruct { }
public class TestClass { }
public enum TestEnum { A }

public class TestTupleRouter<TTuple, TTemplate> : ITupleRouter<TTuple, TTemplate>
    where TTuple : struct, ISpaceTuple
    where TTemplate : struct, ISpaceTemplate
{
    public TTuple Tuple { get; set; } = new();
    public TTemplate Template { get; set; } = new();

    public Task RouteAsync(TTuple tuple)
    {
        Tuple = tuple;
        return Task.CompletedTask;
    }

    public ValueTask RouteAsync(TTemplate template)
    {
        Template = template;
        return ValueTask.CompletedTask;
    }
}

public class TestObserver : SpaceObserver
{
    public SpaceTuple LastTuple { get; private set; } = new();
    public SpaceTemplate LastTemplate { get; private set; } = new();
    public bool LastFlattening { get; private set; }

    public TestObserver() => ListenTo(Everything);

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

public class ThrowingObserver : TestObserver
{
    public override Task OnExpansionAsync(SpaceTuple tuple, CancellationToken cancellationToken)
    {
        throw new Exception("Test");
    }
}

public static class AssertHelpers
{
    public static void AssertEmpty<T>(this T tuple) where T : ISpaceTuple => Assert.Equal(0, tuple.Length);
    public static void AssertNotEmpty<T>(this T tuple) where T : ISpaceTuple => Assert.NotEqual(0, tuple.Length);
}