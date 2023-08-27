using OrleanSpaces.Tuples;
using OrleanSpaces.Tuples.Specialized;
using OrleanSpaces.Collections;

namespace OrleanSpaces.Tests.Collections;

public class ReadOptimizedSpaceCollectionTests
{
    private readonly SpaceTuple tuple = new(1, 'a', 1.5f);
    private readonly SpaceTemplate template = new(1, null, 1.5f);

    [Fact]
    public void Count()
    {
        ReadOptimizedCollection collection = new()
        {
            new(1),
            new('a'),
            new(1, 'a'),
            new(2, 'b'),
            new(1, 'a', 1.5f),
            new(2, 'b', 2.5f)
        };

        Assert.Equal(6, collection.Count);
    }

    [Fact]
    public void Add()
    {
        ReadOptimizedCollection collection = new()
        {
            tuple
        };

        Assert.Single(collection);
    }

    [Fact]
    public void Remove()
    {
        ReadOptimizedCollection collection = new()
        {
            tuple
        };

        collection.Remove(tuple);

        Assert.Empty(collection);
    }

    [Fact]
    public void Clear()
    {
        ReadOptimizedCollection collection = new()
        {
            tuple,
            tuple
        };

        collection.Clear();

        Assert.Empty(collection);
    }

    [Fact]
    public void Find_ReturnsMatchingTuple()
    {
        ReadOptimizedCollection collection = new()
        {
            tuple
        };

        var result = collection.Find(template);

        Assert.Equal(tuple, result);
    }

    [Fact]
    public void Find_ReturnsDefaultWhenNoMatchingTuple()
    {
        ReadOptimizedCollection collection = new();

        var result = collection.Find(template);

        Assert.Equal(result, new());
    }

    [Fact]
    public void FindAll_ReturnsMatchingTuples()
    {
        ReadOptimizedCollection collection = new()
        {
            tuple,
            tuple
        };

        var results = collection.FindAll(template);

        Assert.Equal(2, results.Count());
        Assert.Contains(tuple, results);
    }
}

public class ReadOptimizedIntCollectionTests
{
    private readonly IntTuple tuple = new(1, 2, 3);
    private readonly IntTemplate template = new(1, null, 3);

    [Fact]
    public void Count()
    {
        ReadOptimizedCollection<int, IntTuple, IntTemplate> collection = new()
        {
            new(1),
            new(2),
            new(1, 1),
            new(2, 2),
            new(1, 1, 1),
            new(2, 2, 2)
        };

        Assert.Equal(6, collection.Count);
    }

    [Fact]
    public void Add()
    {
        ReadOptimizedCollection<int, IntTuple, IntTemplate> collection = new()
        {
            tuple
        };

        Assert.Single(collection);
    }

    [Fact]
    public void Remove()
    {
        ReadOptimizedCollection<int, IntTuple, IntTemplate> collection = new()
        {
            tuple
        };

        collection.Remove(tuple);

        Assert.Empty(collection);
    }

    [Fact]
    public void Clear()
    {
        ReadOptimizedCollection<int, IntTuple, IntTemplate> collection = new()
        {
            tuple,
            tuple
        };

        collection.Clear();

        Assert.Empty(collection);
    }

    [Fact]
    public void Find_ReturnsMatchingTuple()
    {
        ReadOptimizedCollection<int, IntTuple, IntTemplate> collection = new()
        {
            tuple
        };

        var result = collection.Find(template);

        Assert.Equal(tuple, result);
    }

    [Fact]
    public void Find_ReturnsDefaultWhenNoMatchingTuple()
    {
        ReadOptimizedCollection<int, IntTuple, IntTemplate> collection = new();

        var result = collection.Find(template);

        Assert.Equal(new(), result);
    }

    [Fact]
    public void FindAll_ReturnsMatchingTuples()
    {
        ReadOptimizedCollection<int, IntTuple, IntTemplate> collection = new()
        {
            tuple,
            tuple
        };

        var results = collection.FindAll(template);

        Assert.Equal(2, results.Count());
        Assert.Contains(tuple, results);
    }
}
