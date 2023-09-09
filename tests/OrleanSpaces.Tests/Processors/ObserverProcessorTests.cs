using OrleanSpaces.Channels;
using OrleanSpaces.Processors;
using OrleanSpaces.Registries;
using OrleanSpaces.Tuples;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Tests.Processors;

public class ObserverSpaceProcessorTests : IClassFixture<ObserverSpaceProcessorTests.Fixture>
{
    private readonly Fixture fixture;
    private readonly ObserverChannel<SpaceTuple> channel;

    public ObserverSpaceProcessorTests(Fixture fixture)
    {
        this.fixture = fixture;
        channel = fixture.Channel;
    }

    [Fact]
    public async Task Should_Notify_All_Observers_On_Expansion_And_Contraction()
    {
        using var scope = fixture.StartScope();

        scope.AddObserver(new TestSpaceObserver<SpaceTuple>());
        scope.AddObserver(new TestSpaceObserver<SpaceTuple>());
        scope.AddObserver(new TestSpaceObserver<SpaceTuple>());

        SpaceTuple tuple = new(1);

        // Expand
        TupleAction<SpaceTuple> insertAction = new(Guid.NewGuid(), tuple.WithDefaultStore(), TupleActionType.Insert);
        await channel.Writer.WriteAsync(insertAction);

        while (scope.TotalInvoked(observer => !observer.LastExpansionTuple.IsEmpty) < 3)
        {

        }

        // Contract
        TupleAction<SpaceTuple> contractAction = new(Guid.NewGuid(), tuple.WithDefaultStore(), TupleActionType.Remove);
        await channel.Writer.WriteAsync(contractAction);

        while (scope.TotalInvoked(observer => !observer.LastContractionTuple.IsEmpty) < 3)
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

        scope.AddObserver(new TestSpaceObserver<SpaceTuple>());
        scope.AddObserver(new TestSpaceObserver<SpaceTuple>());
        scope.AddObserver(new TestSpaceObserver<SpaceTuple>());

        TupleAction<SpaceTuple> cleanAction = new(Guid.NewGuid(), new SpaceTuple(1).WithDefaultStore(), TupleActionType.Clear);
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

public class ObserverIntProcessorTests : IClassFixture<ObserverIntProcessorTests.Fixture>
{
    private readonly Fixture fixture;
    private readonly ObserverChannel<IntTuple> channel;

    public ObserverIntProcessorTests(Fixture fixture)
    {
        this.fixture = fixture;
        channel = fixture.Channel;
    }

    [Fact]
    public async Task Should_Notify_All_Observers_On_Expansion_And_Contraction()
    {
        using var scope = fixture.StartScope();

        scope.AddObserver(new TestSpaceObserver<IntTuple>());
        scope.AddObserver(new TestSpaceObserver<IntTuple>());
        scope.AddObserver(new TestSpaceObserver<IntTuple>());

        IntTuple tuple = new(1);

        // Expand
        TupleAction<IntTuple> insertAction = new(Guid.NewGuid(), tuple.WithDefaultStore(), TupleActionType.Insert);
        await channel.Writer.WriteAsync(insertAction);

        while (scope.TotalInvoked(observer => observer.LastExpansionTuple.Length > 0) < 3)
        {

        }

        // Contract
        TupleAction<IntTuple> contractAction = new(Guid.NewGuid(), tuple.WithDefaultStore(), TupleActionType.Remove);
        await channel.Writer.WriteAsync(contractAction);

        while (scope.TotalInvoked(observer => !observer.LastContractionTuple.IsEmpty) < 3)
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

        scope.AddObserver(new TestSpaceObserver<IntTuple>());
        scope.AddObserver(new TestSpaceObserver<IntTuple>());
        scope.AddObserver(new TestSpaceObserver<IntTuple>());

        TupleAction<IntTuple> cleanAction = new(Guid.NewGuid(), new IntTuple(1).WithDefaultStore(), TupleActionType.Clear);
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
    private readonly Dictionary<Guid, TestSpaceObserver<T>> localRegistry;

    public IEnumerable<TestSpaceObserver<T>> Observers => localRegistry.Values;

    internal TestObserverScope(ObserverRegistry<T> registry)
    {
        this.registry = registry;
        localRegistry = new();
    }

    public int TotalInvoked(Func<TestSpaceObserver<T>, bool> func) => Observers.Count(observer => func(observer));

    public void AddObserver(TestSpaceObserver<T> observer)
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