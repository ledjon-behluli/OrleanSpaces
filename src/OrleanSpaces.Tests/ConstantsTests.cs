namespace OrleanSpaces.Tests;

public class ConstantsTests
{
    [Fact]
    public void Should_Equal()
    {
        Assert.Equal("TupleSpace", Constants.PersistenceStore);
        Assert.Equal("TupleStream", Constants.StreamNamespace);
        Assert.Equal("PubSubProvider", Constants.PubSubProvider);
        Assert.Equal("PubSubStore", Constants.PubSubStore);
    }
}
