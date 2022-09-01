using Microsoft.Extensions.Logging.Abstractions;
using OrleanSpaces.Observers;

namespace OrleanSpaces.Tests.Observers;

public class ProcessorFixture : IDisposable
{
    private readonly ObserverProcessor processor;

    internal ObserverRegistry Registry { get; }
    internal List<TestObserver> Observers { get; }

    public ProcessorFixture()
    {
        Observers = new List<TestObserver>() { new(), new(), new() };

        Registry = new ObserverRegistry();
        Observers.ForEach(x => Registry.Add(x));

        processor = new ObserverProcessor(Registry, new NullLogger<ObserverProcessor>());

        processor.StartAsync(default).Wait();
    }

    public void Dispose() => processor.Dispose();
}
