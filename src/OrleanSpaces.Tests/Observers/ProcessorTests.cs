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
    public async Task Should_Notify_All_Observers_On_Expansion_And_Contraction()
    {
        using var scope = fixture.StartScope();

        scope.AddObserver(new TestObserver());
        scope.AddObserver(new TestObserver());
        scope.AddObserver(new TestObserver());

        // Expand
        SpaceTuple tuple = new((1, "a"));
        await channel.Writer.WriteAsync(tuple);

        while (scope.TotalInvoked(observer => !observer.LastTuple.IsPassive) < 3)
        {

        }

        // Contract
        SpaceTemplate template = tuple;
        await channel.Writer.WriteAsync(template);

        while (scope.TotalInvoked(observer => observer.LastTemplate.IsSatisfiedBy(tuple)) < 3)
        {

        }

        Assert.All(scope.Observers, observer =>
        {
            Assert.Equal(tuple, observer.LastTuple);
            Assert.Equal(template, observer.LastTemplate);
        });
    }

    [Fact]
    public async Task Should_Notify_All_Observers_On_Flattening()
    {
        using var scope = fixture.StartScope();

        scope.AddObserver(new TestObserver());
        scope.AddObserver(new TestObserver());
        scope.AddObserver(new TestObserver());

        await channel.Writer.WriteAsync(SpaceUnit.Null);

        while (scope.TotalInvoked(observer => observer.LastFlattening) < 3)
        {

        }

        Assert.All(scope.Observers, observer =>
            Assert.True(observer.LastFlattening));
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

        while (scope.TotalInvoked(observer => !observer.LastTuple.IsPassive) < 2)
        {

        }

        Assert.All(scope.Observers, observer =>
        {
            if (observer is ThrowingTestObserver)
            {
                Assert.Equal(new(), observer.LastTuple);
            }
            else
            {
                Assert.Equal(tuple, observer.LastTuple);
            }
        });

        Assert.True(hostStopped);
    }
}