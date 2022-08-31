using Microsoft.Extensions.Logging.Abstractions;
using OrleanSpaces.Evaluations;

namespace OrleanSpaces.Tests.Evaluations;

public class ProcessorFixture : IDisposable
{
    private readonly EvaluationProcessor processor;

    public ProcessorFixture()
    {
        processor = new EvaluationProcessor(new NullLogger<EvaluationProcessor>());
        processor.StartAsync(default).Wait();
    }

    public void Dispose() => processor.Dispose();
}
