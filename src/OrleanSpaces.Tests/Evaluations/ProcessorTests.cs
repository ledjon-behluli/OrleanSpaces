using OrleanSpaces.Continuations;
using OrleanSpaces.Evaluations;
using OrleanSpaces.Primitives;
using System.Runtime.CompilerServices;

namespace OrleanSpaces.Tests.Evaluations;

public class ProcessorTests : IClassFixture<Fixture>
{
    private readonly EvaluationChannel evaluationChannel;
    private readonly ContinuationChannel continuationChannel;

    private bool hostStopped;

    public ProcessorTests(Fixture fixture)
    {
        evaluationChannel = fixture.EvaluationChannel;
        continuationChannel = fixture.ContinuationChannel; 

        fixture.Lifetime.ApplicationStopped.Register(() => hostStopped = true);
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

    [Fact]
    public async Task Should_Stop_Host_If_Any_Evaluation_Throws()
    {
        SpaceTuple tuple = new("eval");

        await WriteAsync(tuple, 3, false);
        await WriteAsync(tuple, 2, true);
        await WriteAsync(tuple, 3, false);

        int rounds = 0;

        await foreach (ITuple result in continuationChannel.Reader.ReadAllAsync(default))
        {
            Assert.NotNull(result);
            Assert.True(result is SpaceTuple);
            Assert.Equal(tuple, (SpaceTuple)result);

            rounds++;

            if (continuationChannel.Reader.Count == 0)
            {
                break;
            }
        }

        Assert.Equal(6, rounds);
        Assert.True(hostStopped);

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