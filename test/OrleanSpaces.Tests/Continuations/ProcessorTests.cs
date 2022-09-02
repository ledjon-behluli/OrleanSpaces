using Microsoft.Extensions.Logging.Abstractions;
using OrleanSpaces.Callbacks;
using OrleanSpaces.Continuations;
using OrleanSpaces.Observers;

namespace OrleanSpaces.Tests.Continuations;

public class ProcessorTests : IClassFixture<ProcessorTests.Fixture>
{
    // TODO: Continue

    [Collection(ClusterCollection.Name)]
    public class Fixture : IDisposable
    {
        private readonly ContinuationProcessor processor;

        public Fixture(ClusterFixture fixture)
        {
            processor = new ContinuationProcessor(
                new SpaceChannel(
                    new NullLogger<SpaceChannel>(),
                    fixture.Cluster.Client,
                    new CallbackRegistry(),
                    new ObserverRegistry()),
                new NullLogger<ContinuationProcessor>());

            processor.StartAsync(default).Wait();
        }

        public void Dispose() => processor.Dispose();
    }
}
