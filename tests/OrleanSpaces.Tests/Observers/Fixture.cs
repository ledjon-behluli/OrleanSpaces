using OrleanSpaces.Observers;

namespace OrleanSpaces.Tests.Observers;

public class Fixture : IAsyncLifetime
{
    private readonly ObserverProcessor processor;

    internal ObserverRegistry Registry { get; }
    internal ObserverChannel Channel { get; }

    public Fixture()
    {
        Registry = new();
        Channel = new();
        processor = new(new TestHostAppLifetime(), Registry, Channel);
    }

    public Task InitializeAsync() => processor.StartAsync(default);
    public Task DisposeAsync() => processor.StopAsync(default);


    public TestObserverScope StartScope() => new(Registry);


    public class TestObserverScope : IDisposable
    {
        private readonly ObserverRegistry registry;
        private readonly Dictionary<Guid, TestObserver> localRegistry;

        public IEnumerable<TestObserver> Observers => localRegistry.Values;

        internal TestObserverScope(ObserverRegistry registry)
        {
            this.registry = registry;
            this.localRegistry = new();
        }

        public int TotalInvoked(Func<TestObserver, bool> func) => Observers.Count(observer => func(observer));

        public void AddObserver(TestObserver observer)
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
        }
    }
}
