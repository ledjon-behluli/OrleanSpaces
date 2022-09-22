using OrleanSpaces.Continuations;
using OrleanSpaces.Evaluations;
using OrleanSpaces.Primitives;
using System.Runtime.CompilerServices;

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

        ITuple result = await continuationChannel.Reader.ReadAsync(default);

        Assert.NotNull(result);
        Assert.True(result is SpaceTuple);
        Assert.Equal(tuple, (SpaceTuple)result);
    }

    [Fact]
    public async Task Should_Not_Forward_If_Evaluation_Throws()
    {
        await evaluationChannel.Writer.WriteAsync(() => throw new Exception("Test"));
        continuationChannel.Reader.TryRead(out ITuple result);

        Assert.Null(result);
    }
}