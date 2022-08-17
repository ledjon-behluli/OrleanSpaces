using OrleanSpaces.Types;

namespace OrleanSpaces.Tests;

public class TupleResultTests
{
    [Fact]
    public void Should_Be_False_If_Null()
    {
        TupleResult result = new(null);
        Assert.False(result.Result);
    }

    [Fact]
    public void Should_Be_False_If_Empty_Tuple()
    {
        TupleResult result = TupleResult.Empty;
        Assert.False(result.Result);
    }

    [Fact]
    public void Should_Be_True_If_Real_Tuple()
    {
        TupleResult result = new(SpaceTuple.Create(1));
        Assert.True(result.Result);
    }
}
