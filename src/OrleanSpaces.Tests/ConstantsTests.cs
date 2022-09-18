namespace OrleanSpaces.Tests;

public class ConstantsTests
{
    [Fact]
    public void Should_Equal()
    {
        Assert.Equal("TupleSpaceStore", Constants.TupleSpaceStore);
        Assert.Equal("PubSubProvider", Constants.PubSubProvider);
        Assert.Equal("PubSubStore", Constants.PubSubStore);
        Assert.Equal("TupleSpaceState", Constants.TupleSpaceState);
        Assert.Equal("TupleStream", Constants.TupleStream);
        Assert.Equal(Guid.Empty, Constants.SpaceGrainId);
    }
}
