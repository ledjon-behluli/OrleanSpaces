namespace OrleanSpaces.Analyzers.Tests;

public class ConstantsTests
{
    [Fact]
    public void Should_Equal()
    {
        Assert.Equal("OrleanSpaces.Tuples.SpaceUnit", Constants.SpaceUnitFullName);
        Assert.Equal("OrleanSpaces.Tuples.SpaceTuple", Constants.SpaceTupleFullName);
    }
}

