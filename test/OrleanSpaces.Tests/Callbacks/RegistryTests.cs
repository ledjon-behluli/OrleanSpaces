﻿using OrleanSpaces.Callbacks;
using OrleanSpaces.Primitives;

namespace OrleanSpaces.Tests.Callbacks;

public class RegistryTests
{
    private readonly CallbackRegistry registry = new();

    public RegistryTests()
    {
        registry.Add(SpaceTemplate.Create(1), new(tuple => Task.CompletedTask, false));
        registry.Add(SpaceTemplate.Create(1), new(tuple => Task.CompletedTask, true));
        registry.Add(SpaceTemplate.Create("a"), new(tuple => Task.CompletedTask, false));
    }

    [Fact]
    public void Should_Take_0_Entries()
    {
        var entries = registry.Take(SpaceTuple.Create(1.5));
        Assert.Empty(entries);
    }

    [Fact]
    public void Should_Take_1_Entries()
    {
        var entries = registry.Take(SpaceTuple.Create("a"));
        Assert.Single(entries);
    }

    [Fact]
    public void Should_Take_2_Entries()
    {
        var entries = registry.Take(SpaceTuple.Create(1));
        Assert.Equal(2, entries.Count());
    }

    [Fact]
    public void Should_Get_Callback()
    {
        bool isDestructive = false;
        Func<SpaceTuple, Task> callback = tuple => Task.CompletedTask;

        registry.Add(SpaceTemplate.Create("test"), new(callback, isDestructive));
        var entry = registry.Take(SpaceTuple.Create("test")).ElementAt(0);

        Assert.NotNull(entry);
        Assert.Equal(callback, entry.Callback);
        Assert.Equal(isDestructive, entry.IsDestructive);
    }
}
