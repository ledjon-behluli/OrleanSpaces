using Microsoft.Extensions.Logging.Abstractions;
using OrleanSpaces.Observers;

namespace OrleanSpaces.Tests.Observers;

public class ProcessorFixture : IDisposable
{
    private readonly ObserverProcessor processor;
    private readonly ObserverRegistry registry;

    public ProcessorFixture()
    {
        registry = new ObserverRegistry();
        processor = new ObserverProcessor(registry, new NullLogger<ObserverProcessor>());

        processor.StartAsync(default).Wait();
    }

    public void Dispose() => processor.Dispose();

    public TestObserverScope StartScope() => new(registry);

    public class TestObserverScope : IDisposable
    {
        private readonly ObserverRegistry registry;
        public List<TestObserver> Observers { get; private set; } = new();

        internal TestObserverScope(ObserverRegistry registry)
        {
            this.registry = registry;
        }

        public bool ObserversNotReady(int numOfObservers) =>
            Observers.Count(observer => !observer.LastReceived.IsEmpty) < numOfObservers;

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
