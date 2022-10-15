namespace OrleanSpaces.Analyzers.Tests;

public class FullyQualifiedNamesTests
{
    [Fact]
    public void Should_Equal()
    {
        Assert.Equal("OrleanSpaces.Tuples.SpaceUnit", FullyQualifiedNames.SpaceUnit);
        Assert.Equal("OrleanSpaces.Tuples.SpaceTuple", FullyQualifiedNames.SpaceTuple);
    }
}

