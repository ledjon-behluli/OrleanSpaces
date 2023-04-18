using OrleanSpaces.Callbacks;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Tests.Callbacks;

public class RegistryTests
{
    private readonly CallbackRegistry registry = new();

    public RegistryTests()
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
        bool isContinuable = false;
       
        registry.Add(new("test"), new(callback, isContinuable));
        var entry = registry.Take(new("test")).ElementAt(0);

        Assert.NotNull(entry);
        Assert.Equal(callback, entry.Callback);
        Assert.Equal(isContinuable, entry.IsContinuable);

        static Task callback(SpaceTuple tuple) => Task.CompletedTask;
    }
}
