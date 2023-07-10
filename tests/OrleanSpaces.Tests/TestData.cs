using Orleans.Streams;
using OrleanSpaces.Continuations;
using OrleanSpaces.Observers;
using OrleanSpaces.Tuples;
using OrleanSpaces.Tuples.Specialized;
using System.Collections;

namespace OrleanSpaces.Tests;

internal class SpaceTupleGenerator : IEnumerable<object[]>
{
    private readonly List<object[]> data = new()
    {
        new object[] { new SpaceTuple(1) },
        new object[] { new SpaceTuple(1, "a") },
        new object[] { new SpaceTuple(1, "a", 1.5f) },
        new object[] { new SpaceTuple(1, "a", 1.5f, true) }
    };

    public IEnumerator<object[]> GetEnumerator() => data.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

internal class IntTupleGenerator : IEnumerable<object[]>
{
    private readonly List<object[]> data = new()
    {
        new object[] { new IntTuple(1) },
        new object[] { new IntTuple(1, 2) },
        new object[] { new IntTuple(1, 2, 3) },
        new object[] { new IntTuple(1, 2, 3, 4) }
    };

    public IEnumerator<object[]> GetEnumerator() => data.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

internal class EmptyTupleGenerator : IEnumerable<ISpaceTuple[]>
{
    private readonly List<ISpaceTuple[]> data = new()
    {
        new ISpaceTuple[] { new SpaceTuple() },
        new ISpaceTuple[] { new BoolTuple() },
        new ISpaceTuple[] { new CharTuple() },
        new ISpaceTuple[] { new DateTimeOffsetTuple() },
        new ISpaceTuple[] { new DateTimeTuple() },
        new ISpaceTuple[] { new DecimalTuple() },
        new ISpaceTuple[] { new DoubleTuple() },
        new ISpaceTuple[] { new FloatTuple() },
        new ISpaceTuple[] { new GuidTuple() },
        new ISpaceTuple[] { new HugeTuple() },
        new ISpaceTuple[] { new IntTuple() },
        new ISpaceTuple[] { new LongTuple() },
        new ISpaceTuple[] { new SByteTuple() },
        new ISpaceTuple[] { new ShortTuple() },
        new ISpaceTuple[] { new TimeSpanTuple() },
        new ISpaceTuple[] { new UHugeTuple() },
        new ISpaceTuple[] { new UIntTuple() },
        new ISpaceTuple[] { new ULongTuple() },
        new ISpaceTuple[] { new UShortTuple() }
    };

    public IEnumerator<ISpaceTuple[]> GetEnumerator() => data.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

internal class EmptyTemplateGenerator : IEnumerable<ISpaceTemplate[]>
{
    private readonly List<ISpaceTemplate[]> data = new()
    {
        new ISpaceTemplate[] { new SpaceTemplate() },
        new ISpaceTemplate[] { new BoolTemplate() },
        new ISpaceTemplate[] { new CharTemplate() },
        new ISpaceTemplate[] { new DateTimeOffsetTemplate() },
        new ISpaceTemplate[] { new DateTimeTemplate() },
        new ISpaceTemplate[] { new DecimalTemplate() },
        new ISpaceTemplate[] { new DoubleTemplate() },
        new ISpaceTemplate[] { new FloatTemplate() },
        new ISpaceTemplate[] { new GuidTemplate() },
        new ISpaceTemplate[] { new HugeTemplate() },
        new ISpaceTemplate[] { new IntTemplate() },
        new ISpaceTemplate[] { new LongTemplate() },
        new ISpaceTemplate[] { new SByteTemplate() },
        new ISpaceTemplate[] { new ShortTemplate() },
        new ISpaceTemplate[] { new TimeSpanTemplate() },
        new ISpaceTemplate[] { new UHugeTemplate() },
        new ISpaceTemplate[] { new UIntTemplate() },
        new ISpaceTemplate[] { new ULongTemplate() },
        new ISpaceTemplate[] { new UShortTemplate() }
    };

    public IEnumerator<ISpaceTemplate[]> GetEnumerator() => data.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

internal struct TestStruct { }
internal class TestClass { }
internal enum TestEnum { A }

internal class TestTupleRouter<TTuple, TTemplate> : ITupleRouter<TTuple, TTemplate>
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

internal class TestSpaceObserver<T> : SpaceObserver<T>
    where T : struct, ISpaceTuple
{
    public T LastExpansionTuple { get; protected set; } = new();
    public T LastContractionTuple { get; protected set; } = new();
    public bool HasFlattened { get; protected set; }

    public TestSpaceObserver() => ListenTo(Everything);

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

internal class ThrowingObserver<T> : TestSpaceObserver<T>
     where T : struct, ISpaceTuple
{
    public override Task OnExpansionAsync(T tuple, CancellationToken cancellationToken)
    {
        throw new Exception("Test");
    }
}

internal class TestAsyncObserver<T> : IAsyncObserver<TupleAction<T>>
    where T : struct, ISpaceTuple
{
    public T LastExpansionTuple { get; private set; } = new();
    public T LastContractionTuple { get; private set; } = new();
    public bool HasFlattened { get; private set; }

    public Task OnNextAsync(TupleAction<T> action, StreamSequenceToken? token)
    {
        if (action.Type == TupleActionType.Insert)
        {
            LastExpansionTuple = action.Tuple;
        }
        else if (action.Type == TupleActionType.Remove)
        {
            LastContractionTuple = action.Tuple;
        }
        else if (action.Type == TupleActionType.Clean)
        {
            HasFlattened = true;
        }

        return Task.CompletedTask;
    }

    public Task OnCompletedAsync() => Task.CompletedTask;
    public Task OnErrorAsync(Exception ex) => Task.CompletedTask;

    public void Reset()
    {
        LastExpansionTuple = new();
        LastContractionTuple = new();
        HasFlattened = false;
    }
}

internal static class AssertHelpers
{
    public static void AssertEmpty<T>(this T tuple) where T : ISpaceTuple => Assert.Equal(0, tuple.Length);
    public static void AssertNotEmpty<T>(this T tuple) where T : ISpaceTuple => Assert.NotEqual(0, tuple.Length);
}