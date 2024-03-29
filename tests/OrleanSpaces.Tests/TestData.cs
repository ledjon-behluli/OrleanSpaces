﻿using Orleans.Streams;
using OrleanSpaces.Tuples;
using OrleanSpaces.Tuples.Specialized;
using System.Collections;

namespace OrleanSpaces.Tests;

public class SpaceTupleGenerator : IEnumerable<object[]>
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

public class IntTupleGenerator : IEnumerable<object[]>
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

public class EmptyTupleGenerator : IEnumerable<ISpaceTuple[]>
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

public class EmptyTemplateGenerator : IEnumerable<ISpaceTemplate[]>
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

public struct TestStruct { }
public class TestClass { }
public enum TestEnum { A }

internal class TestSpaceRouter<TTuple, TTemplate> : ISpaceRouter<TTuple, TTemplate>
    where TTuple : struct, ISpaceTuple
    where TTemplate : struct, ISpaceTemplate
{
    public IStoreDirector<TTuple>? Director { get; private set; }
    public TTuple Tuple { get; set; } = new();
    public TTemplate Template { get; set; } = new();
    public TupleAction<TTuple> Action { get; private set; }


    public ValueTask RouteDirector(IStoreDirector<TTuple> director)
    {
        Director = director;
        return ValueTask.CompletedTask;
    }

    public Task RouteTuple(TTuple tuple)
    {
        Tuple = tuple;
        return Task.CompletedTask;
    }

    public ValueTask RouteTemplate(TTemplate template)
    {
        Template = template;
        return ValueTask.CompletedTask;
    }

    public ValueTask RouteAction(TupleAction<TTuple> action)
    {
        Action = action;
        return ValueTask.CompletedTask;
    }
}

public class TestSpaceObserver<T> : SpaceObserver<T>
    where T : struct, ISpaceTuple
{
    public T LastExpansionTuple { get; protected set; } = new();
    public T LastContractionTuple { get; protected set; } = new();
    public bool HasFlattened { get; protected set; }

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

public class ThrowingObserver<T> : TestSpaceObserver<T>
     where T : struct, ISpaceTuple
{
    public override Task OnExpansionAsync(T tuple, CancellationToken cancellationToken)
    {
        throw new Exception("Test");
    }
}

[ImplicitStreamSubscription(Constants.StreamName)]
internal class TestStreamObserver<T> : IAsyncObserver<TupleAction<T>>
    where T : struct, ISpaceTuple
{
    public T LastExpansionTuple { get; private set; } = new();
    public T LastContractionTuple { get; private set; } = new();
    public bool HasFlattened { get; private set; }

    public int InvokedCount { get; private set; }

    public Task OnNextAsync(TupleAction<T> action, StreamSequenceToken? token)
    {
        InvokedCount++;

        if (action.Type == TupleActionType.Insert)
        {
            LastExpansionTuple = action.StoreTuple.Tuple;
        }
        else if (action.Type == TupleActionType.Remove)
        {
            LastContractionTuple = action.StoreTuple.Tuple;
        }
        else if (action.Type == TupleActionType.Clear)
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
        InvokedCount = 0;
    }
}

internal static class Helpers
{
    public static int PartitioningThreshold = new SpaceServerOptions().PartitioningThreshold;
    public static StoreTuple<T> WithDefaultStore<T>(this T tuple) where T : ISpaceTuple => new(Guid.Empty, tuple);
    public static void AssertEmpty<T>(this T tuple) where T : ISpaceTuple => Assert.Equal(0, tuple.Length);
    public static void AssertNotEmpty<T>(this T tuple) where T : ISpaceTuple => Assert.NotEqual(0, tuple.Length);
}