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
        processor = new(Registry, Channel);
    }

    public async Task InitializeAsync() => await processor.StartAsync(default);
    public async Task DisposeAsync() => await processor.StopAsync(default);


    public TestObserverScope StartScope() => new(Registry);


    public class TestObserverScope : IDisposable
    {
        private readonly ObserverRegistry registry;
        public List<TestObserver> Observers { get; private set; } = new();

        internal TestObserverScope(ObserverRegistry registry)
        {
            this.registry = registry;
        }

        public int TotalInvoked(Func<TestObserver, bool> func) => Observers.Count(observer => func(observer));

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
