using OrleanSpaces.Continuations;
using OrleanSpaces.Evaluations;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Tests.Evaluations;

public class ProcessorTests : IClassFixture<Fixture>
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
        SpaceTuple tuple = new("eval");
        await evaluationChannel.Writer.WriteAsync(() => Task.FromResult(tuple));

        ISpaceTuple result = await continuationChannel.Reader.ReadAsync(default);

        Assert.NotNull(result);
        Assert.True(result is SpaceTuple);
        Assert.Equal(tuple, (SpaceTuple)result);
    }

    [Fact]
    public async Task Should_Not_Forward_If_Evaluation_Throws()
    {
        await evaluationChannel.Writer.WriteAsync(() => throw new Exception("Test"));
        continuationChannel.Reader.TryRead(out ISpaceTuple result);

        Assert.Null(result);
    }
}