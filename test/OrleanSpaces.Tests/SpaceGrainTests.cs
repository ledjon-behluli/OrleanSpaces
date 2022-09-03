using Orleans.TestingHost;

namespace OrleanSpaces.Tests;

//public class SpaceGrainTests : IClassFixture<ClusterFixture>
//{
//    private readonly TestCluster cluster;

//    public SpaceGrainTests(ClusterFixture fixture)
//    {
//        cluster = fixture.Cluster;
//    }

//    [Fact]
//    public async Task SaysHelloCorrectly()
//    {
//        ISpaceGrain grain = cluster.GrainFactory.GetSpaceGrain();
//        int tupleCount = await grain.CountAsync();

//        Assert.Equal(0, tupleCount);
//    }
//}