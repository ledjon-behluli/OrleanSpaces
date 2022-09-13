using OrleanSpaces.Observers;
using OrleanSpaces.Primitives;

namespace OrleanSpaces.Tests.Observers;

public class ProcessorTests : IClassFixture<Fixture>
{
    private readonly Fixture fixture;
    private readonly ObserverChannel channel;

    private bool hostStopped;

    public ProcessorTests(Fixture fixture)
	{
        this.fixture = fixture;
        channel = fixture.Channel;

        fixture.Lifetime.ApplicationStopped.Register(() => hostStopped = true);
    }

    [Fact]
    public async Task Should_Notify_All_Observers_OnTuple()
    {
        using var scope = fixture.StartScope();

        scope.AddObserver(new TestObserver());
        scope.AddObserver(new TestObserver());
        scope.AddObserver(new TestObserver());

        SpaceTuple tuple = new(1);
        await channel.Writer.WriteAsync(tuple);

        while (scope.TotalInvoked(observer => !observer.LastReceived.IsUnit) < 3)
        {

        }

        Assert.All(scope.Observers, observer =>
        {
            Assert.False(observer.LastReceived.IsUnit);
            Assert.Equal(tuple, observer.LastReceived);
        });
    }

    [Fact]
    public async Task Should_Notify_All_Observers_OnEmptySpace()
    {
        using var scope = fixture.StartScope();

        scope.AddObserver(new TestObserver());
        scope.AddObserver(new TestObserver());
        scope.AddObserver(new TestObserver());

        await channel.Writer.WriteAsync(SpaceUnit.Null);

        while (scope.TotalInvoked(observer => observer.SpaceEmptiedReceived) < 3)
        {

        }

        Assert.All(scope.Observers, observer =>
            Assert.True(observer.SpaceEmptiedReceived));
    }

    [Fact]
    public async Task Should_Stop_Host_If_Observer_Throws()
    {
        using var scope = fixture.StartScope();

        scope.AddObserver(new TestObserver());
        scope.AddObserver(new ThrowingTestObserver());
        scope.AddObserver(new TestObserver());

        SpaceTuple tuple = new(1);
        await channel.Writer.WriteAsync(tuple);

        while (scope.TotalInvoked(observer => !observer.LastReceived.IsUnit) < 2)
        {

        }

        Assert.All(scope.Observers, observer =>
        {
            if (observer is ThrowingTestObserver)
            {
                Assert.True(observer.LastReceived.IsUnit);
                Assert.Equal(new(), observer.LastReceived);
            }
            else
            {
                Assert.False(observer.LastReceived.IsUnit);
                Assert.Equal(tuple, observer.LastReceived);
            }
        });

        Assert.True(hostStopped);
    }
}