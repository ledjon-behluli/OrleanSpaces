using OrleanSpaces.Callbacks;
using OrleanSpaces.Tuples;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Tests.Callbacks;

public class SpaceRegistryTests
{
    private readonly CallbackRegistry registry = new();

    public SpaceRegistryTests()
    {
        registry.Add(new(1), new(tuple => Task.CompletedTask, false));
        registry.Add(new(1), new(tuple => Task.CompletedTask, true));
        registry.Add(new("a"), new(tuple => Task.CompletedTask, false));
    }

    [Fact]
    public void Should_Take_0_Entries()
    {
        var entries = registry.Take(new(1.5));
        Assert.Empty(entries);
    }

    [Fact]
    public void Should_Take_1_Entries()
    {
        var entries = registry.Take(new("a"));
        Assert.Single(entries);
    }

    [Fact]
    public void Should_Take_2_Entries()
    {
        var entries = registry.Take(new(1));
        Assert.Equal(2, entries.Count());
    }

    [Fact]
    public void Should_Get_Callback()
    {
        bool hasContinuation = false;
       
        registry.Add(new("test"), new(callback, hasContinuation));

        var entry = registry.Take(new("test")).ElementAt(0);
        var defaultEntry = new CallbackEntry<SpaceTuple>();

        Assert.NotEqual(defaultEntry, entry);
        Assert.Equal(callback, entry.Callback);
        Assert.Equal(hasContinuation, entry.HasContinuation);

        static Task callback(SpaceTuple tuple) => Task.CompletedTask;
    }
}

public class IntRegistryTests
{
    private readonly CallbackRegistry<int, IntTuple, IntTemplate> registry = new();

    public IntRegistryTests()
    {
        registry.Add(new(1), new(tuple => Task.CompletedTask, false));
        registry.Add(new(1), new(tuple => Task.CompletedTask, true));
        registry.Add(new(2), new(tuple => Task.CompletedTask, false));
    }

    [Fact]
    public void Should_Take_0_Entries()
    {
        var entries = registry.Take(new(3));
        Assert.Empty(entries);
    }

    [Fact]
    public void Should_Take_1_Entries()
    {
        var entries = registry.Take(new(2));
        Assert.Single(entries);
    }

    [Fact]
    public void Should_Take_2_Entries()
    {
        var entries = registry.Take(new(1));
        Assert.Equal(2, entries.Count());
    }

    [Fact]
    public void Should_Get_Callback()
    {
        bool hasContinuation = false;

        registry.Add(new(0), new(callback, hasContinuation));

        var entry = registry.Take(new(0)).ElementAt(0);
        var defaultEntry = new CallbackEntry<IntTuple>();

        Assert.NotEqual(defaultEntry, entry);
        Assert.Equal(callback, entry.Callback);
        Assert.Equal(hasContinuation, entry.HasContinuation);

        static Task callback(IntTuple tuple) => Task.CompletedTask;
    }
}