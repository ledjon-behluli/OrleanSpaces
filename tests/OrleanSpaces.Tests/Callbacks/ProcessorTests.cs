using OrleanSpaces.Callbacks;
using OrleanSpaces.Continuations;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Tests.Callbacks;

public class ProcessorTests : IClassFixture<Fixture>
{
    private readonly CallbackRegistry registry;
    private readonly CallbackChannel callbackChannel;
    private readonly ContinuationChannel continuationChannel;

    public ProcessorTests(Fixture fixture)
    {
        registry = fixture.Registry;
        callbackChannel = fixture.CallbackChannel;
        continuationChannel = fixture.ContinuationChannel;
    }

    [Fact]
    public async Task Should_Not_Forward_Templates_If_Tuple_Matches_Nothing_In_Registry()
    {
        SpaceTuple tuple = new(1);
        await callbackChannel.Writer.WriteAsync(tuple);

        continuationChannel.Reader.TryRead(out ISpaceTuple result);

        Assert.Null(result);
    }

    [Fact]
    public async Task Should_Forward_Templates_When_Tuple_Matches_Them_In_Registry()
    {
        registry.Add(new(1, "a"), new(tuple => Task.CompletedTask, true));
        registry.Add(new(1, "a"), new(tuple => Task.CompletedTask, false));
        registry.Add(new(1, new SpaceUnit()), new(tuple => Task.CompletedTask, true));
        registry.Add(new(1, new SpaceUnit()), new(tuple => Task.CompletedTask, false));
        registry.Add(new(1, new SpaceUnit()), new(tuple => Task.CompletedTask, true));
        registry.Add(new(1, "a", 1.5F), new(tuple => Task.CompletedTask, true));

        SpaceTuple tuple = new(1, "a");
        await callbackChannel.Writer.WriteAsync(tuple);

        int rounds = 3;

        while (rounds > 0)
        {
            if (continuationChannel.Reader.TryRead(out ISpaceTuple result))
            {
                Assert.NotNull(result);
                Assert.True(result is SpaceTemplate);
                Assert.True(((SpaceTemplate)result).Matches(tuple));

                rounds--;
            }
        }
    }
}