using Microsoft.Extensions.Logging.Abstractions;
using OrleanSpaces.Continuations;
using OrleanSpaces.Evaluations;
using OrleanSpaces.Primitives;

namespace OrleanSpaces.Tests.Evaluations;

public class ProcessorTests : IClassFixture<ProcessorTests.Fixture>
{
    private readonly EvaluationChannel evaluationChannel;
    private readonly ContinuationChannel continuationChannel;

    public ProcessorTests(Fixture fixture)
    {
        evaluationChannel = fixture.EvaluationChannel;
        continuationChannel = fixture.ContinuationChannel;
    }

    [Fact]
    public async Task Should_Forward_If_Evaluation_Results_In_Tuple()
    {
        SpaceTuple tuple = SpaceTuple.Create("eval");
        await evaluationChannel.Writer.WriteAsync(() => Task.FromResult(tuple));

        ISpaceElement element = await continuationChannel.Reader.ReadAsync(default);

        Assert.NotNull(element);
        Assert.True(element is SpaceTuple);
        Assert.Equal(tuple, (SpaceTuple)element);
    }

    [Fact]
    public async Task Should_Not_Forward_If_Evaluation_Throws()
    {
        await evaluationChannel.Writer.WriteAsync(() => throw new Exception("Test"));
        continuationChannel.Reader.TryRead(out ISpaceElement element);

        Assert.Null(element);
    }

    [Fact]
    public async Task Should_Continue_Forwarding_If_Any_Evaluation_Throws()
    {
        SpaceTuple tuple = SpaceTuple.Create("eval");

        await WriteAsync(tuple, 3, false);
        await WriteAsync(tuple, 2, true);
        await WriteAsync(tuple, 3, false);

        int rounds = 0;

        await foreach (var element in continuationChannel.Reader.ReadAllAsync(default))
        {
            Assert.NotNull(element);
            Assert.True(element is SpaceTuple);
            Assert.Equal(tuple, (SpaceTuple)element);

            rounds++;

            if (continuationChannel.Reader.Count == 0)
            {
                break;
            }
        }

        Assert.Equal(6, rounds);

        async Task WriteAsync(SpaceTuple tuple, int times, bool doThrow)
        {
            for (int i = 0; i < times; i++)
            {
                await evaluationChannel.Writer.WriteAsync(
                    () => doThrow ? throw new Exception("Test") : Task.FromResult(tuple));
            }
        }
    }

    public class Fixture : IAsyncLifetime
    {
        private readonly EvaluationProcessor processor;

        internal EvaluationChannel EvaluationChannel { get; }
        internal ContinuationChannel ContinuationChannel { get; }

        public Fixture()
        {
            EvaluationChannel = new();
            ContinuationChannel = new();
            processor = new(EvaluationChannel, ContinuationChannel, new NullLogger<EvaluationProcessor>());
        }

        public async Task InitializeAsync() => await processor.StartAsync(default);
        public async Task DisposeAsync() => await processor.StopAsync(default);
    }
}