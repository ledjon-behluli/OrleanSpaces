namespace OrleanSpaces.Analyzers.Tests;

public class ConstantsTests
{
    [Fact]
    public void Should_Equal_FullNames()
    {
        Assert.Equal("OrleanSpaces.Tuples.SpaceUnit", FullyQualifiedNames.SpaceUnit);
        Assert.Equal("OrleanSpaces.Tuples.SpaceTuple", FullyQualifiedNames.SpaceTuple);
        Assert.Equal("OrleanSpaces.Tuples.SpaceTemplate", FullyQualifiedNames.SpaceTemplate);
    }
}

