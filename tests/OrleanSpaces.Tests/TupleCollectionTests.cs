using OrleanSpaces.Tuples;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Tests;

public class SpaceTupleCollectionTests
{
    private readonly SpaceTuple tuple = new(1, 'a', 1.5f);
    private readonly SpaceTemplate template = new(1, null, 1.5f);

    [Fact]
    public void Count()
    {
        TupleCollection collection = new()
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
        TupleCollection collection = new()
        {
            tuple
        };

        Assert.Single(collection);
    }

    [Fact]
    public void Remove()
    {
        TupleCollection collection = new()
        {
            tuple
        };

        collection.Remove(tuple);

        Assert.Empty(collection);
    }

    [Fact]
    public void Clear()
    {
        TupleCollection collection = new()
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
        TupleCollection collection = new()
        {
            tuple
        };

        var result = collection.Find(template);

        Assert.Equal(tuple, result);
    }

    [Fact]
    public void Find_ReturnsDefaultWhenNoMatchingTuple()
    {
        TupleCollection collection = new();

        var result = collection.Find(template);

        Assert.Equal(result, new());
    }

    [Fact]
    public void FindAll_ReturnsMatchingTuples()
    {
        TupleCollection collection = new()
        {
            tuple,
            tuple
        };

        var results = collection.FindAll(template);

        Assert.Equal(2, results.Count());
        Assert.Contains(tuple, results);
    }
}

public class IntTupleCollectionTests
{
    private readonly IntTuple tuple = new(1, 2, 3);
    private readonly IntTemplate template = new(1, null, 3);

    [Fact]
    public void Count()
    {
        TupleCollection<int, IntTuple, IntTemplate> collection = new()
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
        TupleCollection<int, IntTuple, IntTemplate> collection = new()
        {
            tuple
        };

        Assert.Single(collection);
    }

    [Fact]
    public void Remove()
    {
        TupleCollection<int, IntTuple, IntTemplate> collection = new()
        {
            tuple
        };

        collection.Remove(tuple);

        Assert.Empty(collection);
    }

    [Fact]
    public void Clear()
    {
        TupleCollection<int, IntTuple, IntTemplate> collection = new()
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
        TupleCollection<int, IntTuple, IntTemplate> collection = new()
        {
            tuple
        };

        var result = collection.Find(template);

        Assert.Equal(tuple, result);
    }

    [Fact]
    public void Find_ReturnsDefaultWhenNoMatchingTuple()
    {
        TupleCollection<int, IntTuple, IntTemplate> collection = new();

        var result = collection.Find(template);

        Assert.Equal(new(), result);
    }

    [Fact]
    public void FindAll_ReturnsMatchingTuples()
    {
        TupleCollection<int, IntTuple, IntTemplate> collection = new()
        {
            tuple,
            tuple
        };

        var results = collection.FindAll(template);

        Assert.Equal(2, results.Count());
        Assert.Contains(tuple, results);
    }
}
