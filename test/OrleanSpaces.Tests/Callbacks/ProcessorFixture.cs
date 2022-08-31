using Microsoft.Extensions.Logging.Abstractions;
using OrleanSpaces.Callbacks;
using OrleanSpaces.Primitives;

namespace OrleanSpaces.Tests.Callbacks;

public class ProcessorFixture : IDisposable
{
    private readonly CallbackProcessor processor;

    public ProcessorFixture()
    {
        CallbackRegistry registry = new();

        registry.Add(SpaceTemplate.Create("a"), new(tuple => Task.CompletedTask, false));
        registry.Add(SpaceTemplate.Create(("a", 1)), new(tuple => Task.CompletedTask, true));
        registry.Add(SpaceTemplate.Create(("a", 1, UnitField.Null)), new(tuple => Task.CompletedTask, false));

        processor = new CallbackProcessor(registry, new NullLogger<CallbackProcessor>());
        processor.StartAsync(default).Wait();
    }

    public void Dispose() => processor.Dispose();
}
