using Microsoft.Extensions.Logging.Abstractions;
using OrleanSpaces.Observers;
using OrleanSpaces.Primitives;

namespace OrleanSpaces.Tests.Observers;

public class ProcessorTests : IClassFixture<ProcessorTests.Fixture>
{
    private readonly Fixture fixture;
    private readonly ObserverChannel channel;

	public ProcessorTests(Fixture fixture)
	{
        this.fixture = fixture;
        channel = fixture.Channel;
	}

    [Fact]
    public async void Should_Notify_All_Observers()
    {
        using var scope = fixture.StartScope();

        scope.AddObserver(new TestObserver());
        scope.AddObserver(new TestObserver());
        scope.AddObserver(new TestObserver());

        SpaceTuple tuple = SpaceTuple.Create(1);
        await channel.Writer.WriteAsync(tuple);

        while (scope.TotalInvoked() < 3)
        {

        }

        Assert.All(scope.Observers, observer =>
        {
            Assert.False(observer.LastReceived.IsEmpty);
            Assert.Equal(tuple, observer.LastReceived);
        });
    }

    [Fact]
    public async void Should_Continue_To_Notify_Other_Observers_When_Some_Throw()
    {
        using var scope = fixture.StartScope();

        scope.AddObserver(new TestObserver());
        scope.AddObserver(new ThrowingTestObserver());
        scope.AddObserver(new TestObserver());

        SpaceTuple tuple = SpaceTuple.Create(1);
        await channel.Writer.WriteAsync(tuple);

        while (scope.TotalInvoked() < 2)
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

    public class Fixture : IAsyncLifetime
    {
        private readonly ObserverProcessor processor;

        internal ObserverRegistry Registry { get; }
        internal ObserverChannel Channel { get; }

        public Fixture()
        {
            Registry = new();
            Channel = new();
            processor = new(Registry, Channel, new NullLogger<ObserverProcessor>());
        }

        public Task InitializeAsync() => processor.StartAsync(default);
        public Task DisposeAsync() => processor.StopAsync(default);


        public TestObserverScope StartScope() => new(Registry);


        public class TestObserverScope : IDisposable
        {
            private readonly ObserverRegistry registry;
            public List<TestObserver> Observers { get; private set; } = new();

            internal TestObserverScope(ObserverRegistry registry)
            {
                this.registry = registry;
            }

            public int TotalInvoked() => Observers.Count(observer => !observer.LastReceived.IsEmpty);

            public void AddObserver(TestObserver observer)
            {
                Observers.Add(observer);
                registry.Add(observer);
            }

            public void Dispose()
            {
                Observers.ForEach(x => registry.Remove(x));
                Observers.Clear();
            }
        }
    }
}
