using OrleanSpaces.Continuations;
using OrleanSpaces.Evaluations;
using OrleanSpaces.Primitives;

namespace OrleanSpaces.Tests.Evaluations;

public class ProcessorTests : IClassFixture<ProcessorFixture>
{
    [Fact]
    public async Task Should_Forward_Tuple_To_ContinuationChannel()
    {
        SpaceTuple tuple = SpaceTuple.Create(1);
        await EvaluationChannel.Writer.WriteAsync(() => Task.FromResult(tuple));
        ISpaceElement element = await ContinuationChannel.Reader.ReadAsync(default);

        Assert.NotNull(element);
        Assert.True(element is SpaceTuple);
        Assert.Equal(tuple, (SpaceTuple)element);
    }

    [Fact]
    public async Task Should_Ignore_Evaluation_That_Throws()
    {
        await EvaluationChannel.Writer.WriteAsync(() => throw new Exception("Test"));
        ContinuationChannel.Reader.TryRead(out ISpaceElement element);

        Assert.Null(element);
    }
}