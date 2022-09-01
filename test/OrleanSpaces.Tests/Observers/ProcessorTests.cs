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
        using (var scope = fixture.StartScope())
        {
            scope.AddObserver(new TestObserver());
            scope.AddObserver(new TestObserver());
            scope.AddObserver(new TestObserver());

            SpaceTuple tuple = SpaceTuple.Create(1);
            await ObserverChannel.Writer.WriteAsync(tuple);

            while (scope.ObserversNotReady(3))
            {

            }

            Assert.All(scope.Observers, observer =>
            {
                Assert.False(observer.LastReceived.IsEmpty);
                Assert.Equal(tuple, observer.LastReceived);
            });
        }
    }

    [Fact]
    public async void Should_Continue_To_Notify_Other_Observers_When_Some_Throw()
    {
        using (var scope = fixture.StartScope())
        {
            scope.AddObserver(new TestObserver());
            scope.AddObserver(new ThrowingTestObserver());
            scope.AddObserver(new TestObserver());

            SpaceTuple tuple = SpaceTuple.Create(1);
            await ObserverChannel.Writer.WriteAsync(tuple);

            while (scope.ObserversNotReady(2))
            {

            }

            Assert.All(scope.Observers, observer =>
            {
                if (observer is ThrowingTestObserver)
                {
                    Assert.True(observer.LastReceived.IsEmpty);
                    Assert.Equal(default, observer.LastReceived);
                }
                else
                {
                    Assert.False(observer.LastReceived.IsEmpty);
                    Assert.Equal(tuple, observer.LastReceived);
                }
            });
        }
    }
}
