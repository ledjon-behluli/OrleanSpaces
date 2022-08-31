using OrleanSpaces.Continuations;
using OrleanSpaces.Evaluations;
using OrleanSpaces.Primitives;

namespace OrleanSpaces.Tests.Evaluations;

public class ProcessorTests : IClassFixture<ProcessorFixture>
{
    [Fact]
    public async Task Should_Forward_If_Evaluation_Results_In_Tuple()
    {
        SpaceTuple tuple = SpaceTuple.Create(1);
        await EvaluationChannel.Writer.WriteAsync(() => Task.FromResult(tuple));

        ISpaceElement element = await ContinuationChannel.Reader.ReadAsync(default);

        Assert.NotNull(element);
        Assert.True(element is SpaceTuple);
        Assert.Equal(tuple, (SpaceTuple)element);
    }

    [Fact]
    public async Task Should_Not_Forward_If_Evaluation_Throws()
    {
        await EvaluationChannel.Writer.WriteAsync(() => throw new Exception("Test"));
        ContinuationChannel.Reader.TryRead(out ISpaceElement element);

        Assert.Null(element);
    }

    [Fact]
    public async Task Should_Not_Stop_Forwarding_If_Any_Evaluation_Throws()
    {
        SpaceTuple tuple = SpaceTuple.Create(1);

        await WriteAsync(tuple, 3, false);
        await WriteAsync(tuple, 2, true);
        await WriteAsync(tuple, 3, false);

        int rounds = 0;

        await foreach (var element in ContinuationChannel.Reader.ReadAllAsync(default))
        {
            Assert.NotNull(element);
            Assert.True(element is SpaceTuple);
            Assert.Equal(tuple, (SpaceTuple)element);

            rounds++;

            if (ContinuationChannel.Reader.Count == 0)
            {
                break;
            }
        }

        Assert.Equal(6, rounds);

        static async Task WriteAsync(SpaceTuple tuple, int times, bool doThrow)
        {
            for (int i = 0; i < times; i++)
            {
                await EvaluationChannel.Writer.WriteAsync(
                    () => doThrow ? throw new Exception("Test") : Task.FromResult(tuple));
            }
        }
    }
}