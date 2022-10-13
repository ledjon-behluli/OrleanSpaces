namespace OrleanSpaces.Analyzers.Tests;

public class ConstantsTests
{
    [Fact]
    public void Should_Equal()
    {
        Assert.Equal("DefaultableAttribute", Constants.Defaultable_Attribute_Name);
        Assert.Equal("OrleanSpaces.Tuples", Constants.OrleanSpaces_Tuples_Namespace);
    }
}

