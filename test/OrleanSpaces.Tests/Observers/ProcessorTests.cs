using OrleanSpaces.Observers;
using OrleanSpaces.Primitives;

namespace OrleanSpaces.Tests.Observers;

public class ProcessorTests : IClassFixture<ProcessorFixture>
{
    private readonly ProcessorFixture fixture;

	public ProcessorTests(ProcessorFixture fixture)
	{
        this.fixture = fixture;
	}

    [Fact]
    public async void Should_Notify_All_Observers()
    {
        SpaceTuple tuple = SpaceTuple.Create(1);
        await ObserverChannel.Writer.WriteAsync(tuple);

        while (fixture.Observers.Any(observer => observer.LastReceived.IsEmpty))
        {

        }

        Assert.All(fixture.Observers, observer =>
        {
            Assert.False(observer.LastReceived.IsEmpty);
            Assert.Equal(tuple, observer.LastReceived);
        });
    }

    [Fact]
    public async void Should_Continue_To_Notify_Other_Observers_When_Some_Throw()
    {
        fixture.Registry.Add(new ThrowingTestObserver());
        fixture.Registry.Add(new ThrowingTestObserver());

        SpaceTuple tuple = SpaceTuple.Create(1);
        await ObserverChannel.Writer.WriteAsync(tuple);

        while (fixture.Observers.Any(observer => observer.LastReceived.IsEmpty))
        {

        }

        var a = fixture.Observers.Where(x => x is ThrowingTestObserver);
        var v = fixture.Observers.Where(x => x is TestObserver);

        Assert.All(fixture.Observers.Where(x => x is TestObserver), observer =>
        {
            Assert.False(observer.LastReceived.IsEmpty);
            Assert.Equal(tuple, observer.LastReceived);
        });

        Assert.All(fixture.Observers.Where(x => x is ThrowingTestObserver), observer =>
        {
            Assert.True(observer.LastReceived.IsEmpty);
            Assert.Equal(new(), observer.LastReceived);
        });
    }
}
