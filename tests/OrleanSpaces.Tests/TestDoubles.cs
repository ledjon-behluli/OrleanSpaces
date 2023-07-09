using OrleanSpaces.Continuations;
using OrleanSpaces.Observers;
using OrleanSpaces.Tuples;

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

public class TestObserver<T> : SpaceObserver<T>
    where T : struct, ISpaceTuple
{
    public T LastExpansionTuple { get; protected set; } = new();
    public T LastContractionTuple { get; protected set; } = new();
    public bool HasFlattened { get; protected set; }

    public TestObserver() => ListenTo(Everything);

    public override Task OnExpansionAsync(T tuple, CancellationToken cancellationToken)
    {
        LastExpansionTuple = tuple;
        return Task.CompletedTask;
    }

    public override Task OnContractionAsync(T tuple, CancellationToken cancellationToken)
    {
        LastContractionTuple = tuple;
        return Task.CompletedTask;
    }

    public override Task OnFlatteningAsync(CancellationToken cancellationToken)
    {
        HasFlattened = true;
        return Task.CompletedTask;
    }
}

public class ThrowingObserver<T> : TestObserver<T>
     where T : struct, ISpaceTuple
{
    public override Task OnExpansionAsync(T tuple, CancellationToken cancellationToken)
    {
        throw new Exception("Test");
    }
}

public static class AssertHelpers
{
    public static void AssertEmpty<T>(this T tuple) where T : ISpaceTuple => Assert.Equal(0, tuple.Length);
    public static void AssertNotEmpty<T>(this T tuple) where T : ISpaceTuple => Assert.NotEqual(0, tuple.Length);
}