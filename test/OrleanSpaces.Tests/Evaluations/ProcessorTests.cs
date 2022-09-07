using OrleanSpaces.Continuations;
using OrleanSpaces.Evaluations;
using OrleanSpaces.Primitives;

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
        SpaceTuple tuple = SpaceTuple.Create("eval");
        await evaluationChannel.Writer.WriteAsync(() => Task.FromResult(tuple));

        ISpaceTuple spaceTuple = await continuationChannel.Reader.ReadAsync(default);

        Assert.NotNull(spaceTuple);
        Assert.True(spaceTuple is SpaceTuple);
        Assert.Equal(tuple, (SpaceTuple)spaceTuple);
    }

    [Fact]
    public async Task Should_Not_Forward_If_Evaluation_Throws()
    {
        await evaluationChannel.Writer.WriteAsync(() => throw new Exception("Test"));
        continuationChannel.Reader.TryRead(out ISpaceTuple spaceTuple);

        Assert.Null(spaceTuple);
    }

    [Fact]
    public async Task Should_Continue_Forwarding_If_Any_Evaluation_Throws()
    {
        SpaceTuple tuple = SpaceTuple.Create("eval");

        await WriteAsync(tuple, 3, false);
        await WriteAsync(tuple, 2, true);
        await WriteAsync(tuple, 3, false);

        int rounds = 0;

        await foreach (ISpaceTuple spaceTuple in continuationChannel.Reader.ReadAllAsync(default))
        {
            Assert.NotNull(spaceTuple);
            Assert.True(spaceTuple is SpaceTuple);
            Assert.Equal(tuple, (SpaceTuple)spaceTuple);

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
}