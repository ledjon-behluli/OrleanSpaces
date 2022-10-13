namespace OrleanSpaces.Analyzers.Tests;

public class ConstantsTests
{
    [Fact]
    public void Should_Equal()
    {
        Assert.Equal("DefaultableAttribute", Constants.DefaultableAttributeName);
        Assert.Equal("OrleanSpaces.Tuples.SpaceTuple", Constants.SpaceTupleFullName);
    }
}

