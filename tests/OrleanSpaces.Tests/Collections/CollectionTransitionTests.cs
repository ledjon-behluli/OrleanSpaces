using OrleanSpaces.Collections;
using OrleanSpaces.Helpers;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Tests.Collections;

public class CollectionTransitionTests
{
    [Fact]
    public void EmptyCollection_ShouldReturnZeroValues()
    {
        WriteOptimizedCollection collection = new();

        var result = collection.Calculate(default);

        Assert.Equal(0, result.TupleLengthMean);
        Assert.Equal(0, result.TupleLengthRelativeStdDev);
    }

    [Fact]
    public void NonEmptyCollection_ShouldCalculateCorrectly()
    {
        WriteOptimizedCollection collection = new();
        CollectionStatistics statistics = default;

        collection.Add(new SpaceTuple(1));
        collection.Add(new SpaceTuple(2));
        collection.Add(new SpaceTuple(3));

        collection.Add(new SpaceTuple(1, 1));
        collection.Add(new SpaceTuple(2, 2));
        collection.Add(new SpaceTuple(3, 3));

        collection.Add(new SpaceTuple(1, 1, 1));
        collection.Add(new SpaceTuple(2, 2, 2));
        collection.Add(new SpaceTuple(3, 3, 3));

        statistics = collection.Calculate(statistics);

        Assert.Equal(2.0, Math.Round(statistics.TupleLengthMean, 1));
        Assert.Equal(2, statistics.TupleLengthRelativeStdDev); //todo: fix
    }
}