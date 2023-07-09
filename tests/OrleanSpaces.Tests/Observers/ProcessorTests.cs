using OrleanSpaces.Observers;
using OrleanSpaces.Tuples;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Tests.Observers;

public class SpaceProcessorTests : IClassFixture<SpaceProcessorTests.Fixture>
{
    private readonly Fixture fixture;
    private readonly ObserverChannel<SpaceTuple> channel;

    public SpaceProcessorTests(Fixture fixture)
	{
        this.fixture = fixture;
        channel = fixture.Channel;
    }

    [Fact]
    public async Task Should_Notify_All_Observers_On_Expansion_And_Contraction()
    {
        using var scope = fixture.StartScope();

        scope.AddObserver(new TestObserver<SpaceTuple>());
        scope.AddObserver(new TestObserver<SpaceTuple>());
        scope.AddObserver(new TestObserver<SpaceTuple>());

        SpaceTuple tuple = new(1);

        // Expand
        TupleAction<SpaceTuple> insertAction = new(Guid.NewGuid(), tuple, TupleActionType.Insert);
        await channel.Writer.WriteAsync(insertAction);

        while (scope.TotalInvoked(observer => observer.LastExpansionTuple.Length > 0) < 3)
        {

        }

        // Contract
        TupleAction<SpaceTuple> contractAction = new(Guid.NewGuid(), tuple, TupleActionType.Remove);
        await channel.Writer.WriteAsync(contractAction);

        while (scope.TotalInvoked(observer => observer.LastContractionTuple.Length > 0) < 3)
        {

        }

        Assert.All(scope.Observers, observer =>
        {
            Assert.Equal(tuple, observer.LastExpansionTuple);
            Assert.Equal(tuple, observer.LastContractionTuple);
        });
    }

    [Fact]
    public async Task Should_Notify_All_Observers_On_Flattening()
    {
        using var scope = fixture.StartScope();

        scope.AddObserver(new TestObserver<SpaceTuple>());
        scope.AddObserver(new TestObserver<SpaceTuple>());
        scope.AddObserver(new TestObserver<SpaceTuple>());

        TupleAction<SpaceTuple> cleanAction = new(Guid.NewGuid(), new(1), TupleActionType.Clean);
        await channel.Writer.WriteAsync(cleanAction);

        while (scope.TotalInvoked(observer => observer.HasFlattened) < 3)
        {

        }

        Assert.All(scope.Observers, observer => Assert.True(observer.HasFlattened));
    }

    public class Fixture : IAsyncLifetime
    {
        private readonly ObserverProcessor<SpaceTuple> processor;

        internal ObserverRegistry<SpaceTuple> Registry { get; }
        internal ObserverChannel<SpaceTuple> Channel { get; }

        public Fixture()
        {
            Registry = new();
            Channel = new();
            processor = new(Registry, Channel);
        }

        public Task InitializeAsync() => processor.StartAsync(default);
        public Task DisposeAsync() => processor.StopAsync(default);


        public TestObserverScope<SpaceTuple> StartScope() => new(Registry);
    }
}

public class IntProcessorTests : IClassFixture<IntProcessorTests.Fixture>
{
    private readonly Fixture fixture;
    private readonly ObserverChannel<IntTuple> channel;

    public IntProcessorTests(Fixture fixture)
    {
        this.fixture = fixture;
        channel = fixture.Channel;
    }

    [Fact]
    public async Task Should_Notify_All_Observers_On_Expansion_And_Contraction()
    {
        using var scope = fixture.StartScope();

        scope.AddObserver(new TestObserver<IntTuple>());
        scope.AddObserver(new TestObserver<IntTuple>());
        scope.AddObserver(new TestObserver<IntTuple>());

        IntTuple tuple = new(1);

        // Expand
        TupleAction<IntTuple> insertAction = new(Guid.NewGuid(), tuple, TupleActionType.Insert);
        await channel.Writer.WriteAsync(insertAction);

        while (scope.TotalInvoked(observer => observer.LastExpansionTuple.Length > 0) < 3)
        {

        }

        // Contract
        TupleAction<IntTuple> contractAction = new(Guid.NewGuid(), tuple, TupleActionType.Remove);
        await channel.Writer.WriteAsync(contractAction);

        while (scope.TotalInvoked(observer => observer.LastContractionTuple.Length > 0) < 3)
        {

        }

        Assert.All(scope.Observers, observer =>
        {
            Assert.Equal(tuple, observer.LastExpansionTuple);
            Assert.Equal(tuple, observer.LastContractionTuple);
        });
    }

    [Fact]
    public async Task Should_Notify_All_Observers_On_Flattening()
    {
        using var scope = fixture.StartScope();

        scope.AddObserver(new TestObserver<IntTuple>());
        scope.AddObserver(new TestObserver<IntTuple>());
        scope.AddObserver(new TestObserver<IntTuple>());

        TupleAction<IntTuple> cleanAction = new(Guid.NewGuid(), new(1), TupleActionType.Clean);
        await channel.Writer.WriteAsync(cleanAction);

        while (scope.TotalInvoked(observer => observer.HasFlattened) < 3)
        {

        }

        Assert.All(scope.Observers, observer => Assert.True(observer.HasFlattened));
    }

    public class Fixture : IAsyncLifetime
    {
        private readonly ObserverProcessor<IntTuple> processor;

        internal ObserverRegistry<IntTuple> Registry { get; }
        internal ObserverChannel<IntTuple> Channel { get; }

        public Fixture()
        {
            Registry = new();
            Channel = new();
            processor = new(Registry, Channel);
        }

        public Task InitializeAsync() => processor.StartAsync(default);
        public Task DisposeAsync() => processor.StopAsync(default);


        public TestObserverScope<IntTuple> StartScope() => new(Registry);
    }
}

public class TestObserverScope<T> : IDisposable
    where T : struct, ISpaceTuple
{
    private readonly ObserverRegistry<T> registry;
    private readonly Dictionary<Guid, TestObserver<T>> localRegistry;

    public IEnumerable<TestObserver<T>> Observers => localRegistry.Values;

    internal TestObserverScope(ObserverRegistry<T> registry)
    {
        this.registry = registry;
        this.localRegistry = new();
    }

    public int TotalInvoked(Func<TestObserver<T>, bool> func) => Observers.Count(observer => func(observer));

    public void AddObserver(TestObserver<T> observer)
    {
        localRegistry.Add(Guid.NewGuid(), observer);
        registry.Add(observer);
    }

    public void Dispose()
    {
        foreach (Guid key in localRegistry.Keys)
        {
            registry.Remove(key);
        }

        localRegistry.Clear();
        GC.SuppressFinalize(this);
    }
}