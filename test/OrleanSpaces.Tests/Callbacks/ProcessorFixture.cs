using Microsoft.Extensions.Logging.Abstractions;
using OrleanSpaces.Callbacks;

namespace OrleanSpaces.Tests.Callbacks;

public class ProcessorFixture : IDisposable
{
    private readonly CallbackProcessor processor;
    internal CallbackRegistry Registry { get; }

    public ProcessorFixture()
    {
        Registry = new CallbackRegistry();
        processor = new CallbackProcessor(Registry, new NullLogger<CallbackProcessor>());

        processor.StartAsync(default).Wait();
    }

    public void Dispose() => processor.Dispose();
}
