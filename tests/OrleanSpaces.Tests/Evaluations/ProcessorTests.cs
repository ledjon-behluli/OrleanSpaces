using OrleanSpaces.Continuations;
using OrleanSpaces.Evaluations;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Tests.Evaluations;

public class SpaceProcessorTests : IClassFixture<SpaceProcessorTests.Fixture>
{
    private readonly SpaceOptions options;
    private readonly EvaluationChannel<SpaceTuple> evaluationChannel;
    private readonly ContinuationChannel<SpaceTuple, SpaceTemplate> continuationChannel;

    public SpaceProcessorTests(Fixture fixture)
    {
        options = fixture.Options;
        evaluationChannel = fixture.EvaluationChannel;
        continuationChannel = fixture.ContinuationChannel; 
    }

    [Fact]
    public async Task Should_Forward_If_Evaluation_Results_In_Tuple()
    {
        SpaceTuple tuple = new("eval");
        await evaluationChannel.Writer.WriteAsync(() => Task.FromResult(tuple));

        SpaceTuple result = await continuationChannel.TupleReader.ReadAsync(default);

        result.AssertNotEmpty();
        Assert.Equal(tuple, result);
    }

    [Fact]
    public async Task Should_Not_Forward_If_Evaluation_Throws()
    {
        options.IgnoreEvaluationExceptions = true;

        await evaluationChannel.Writer.WriteAsync(() => throw new Exception("Test"));
        continuationChannel.TupleReader.TryRead(out SpaceTuple result);

        result.AssertEmpty();

        options.IgnoreEvaluationExceptions = false;
    }

    [Fact]
    public void Should_Throw_If_Evaluation_Throws()
    {
        options.IgnoreEvaluationExceptions = false;
       
        _ = Assert.ThrowsAsync<Exception>(async () => await evaluationChannel.Writer.WriteAsync(() => throw new Exception("Test")));
        
        options.IgnoreEvaluationExceptions = true;
    }

    public class Fixture : IAsyncLifetime
    {
        private readonly EvaluationProcessor<SpaceTuple, SpaceTemplate> processor;

        internal SpaceOptions Options { get; }
        internal EvaluationChannel<SpaceTuple> EvaluationChannel { get; }
        internal ContinuationChannel<SpaceTuple, SpaceTemplate> ContinuationChannel { get; }

        public Fixture()
        {
            Options = new();
            EvaluationChannel = new();
            ContinuationChannel = new();
            processor = new(Options, EvaluationChannel, ContinuationChannel);
        }

        public Task InitializeAsync() => processor.StartAsync(default);
        public Task DisposeAsync() => processor.StopAsync(default);
    }
}