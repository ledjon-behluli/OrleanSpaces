using OrleanSpaces.Callbacks;
using OrleanSpaces.Continuations;
using OrleanSpaces.Primitives;
using System.Runtime.CompilerServices;

namespace OrleanSpaces.Tests.Callbacks;

public class ProcessorTests : IClassFixture<Fixture>
{
    private readonly CallbackRegistry registry;
    private readonly CallbackChannel callbackChannel;
    private readonly ContinuationChannel continuationChannel;

    private bool hostStopped;

    public ProcessorTests(Fixture fixture)
    {
        registry = fixture.Registry;
        callbackChannel = fixture.CallbackChannel;
        continuationChannel = fixture.ContinuationChannel;

        fixture.Lifetime.ApplicationStopped.Register(() => hostStopped = true);
    }

    [Fact]
    public async Task Should_Not_Forward_Templates_If_Tuple_Matches_Nothing_In_Registry()
    {
        SpaceTuple tuple = SpaceTuple.Create(1);
        await callbackChannel.Writer.WriteAsync(tuple);

        continuationChannel.Reader.TryRead(out ITuple result);

        Assert.Null(result);
    }

    [Fact]
    public async Task Should_Forward_Templates_When_Tuple_Matches_Them_In_Registry()
    {
        registry.Add(SpaceTemplate.Create((1, "a")), new(tuple => Task.CompletedTask, true));
        registry.Add(SpaceTemplate.Create((1, "a")), new(tuple => Task.CompletedTask, false));
        registry.Add(SpaceTemplate.Create((1, SpaceUnit.Null)), new(tuple => Task.CompletedTask, true));
        registry.Add(SpaceTemplate.Create((1, SpaceUnit.Null)), new(tuple => Task.CompletedTask, false));
        registry.Add(SpaceTemplate.Create((1, SpaceUnit.Null)), new(tuple => Task.CompletedTask, true));
        registry.Add(SpaceTemplate.Create((1, "a", 1.5F)), new(tuple => Task.CompletedTask, true));

        SpaceTuple tuple = SpaceTuple.Create((1, "a"));
        await callbackChannel.Writer.WriteAsync(tuple);

        int rounds = 0;

        await foreach (ITuple result in continuationChannel.Reader.ReadAllAsync(default))
        {
            Assert.NotNull(result);
            Assert.True(result is SpaceTemplate);
            Assert.True(((SpaceTemplate)result).IsSatisfiedBy(tuple));

            rounds++;

            if (continuationChannel.Reader.Count == 0)
            {
                break;
            }
        }

        Assert.Equal(3, rounds);
    }

    [Fact]
    public async Task Should_Stop_Host_If_Callback_Throws()
    {
        registry.Add(SpaceTemplate.Create((1, "a")), new(tuple => Task.CompletedTask, true));
        registry.Add(SpaceTemplate.Create((1, SpaceUnit.Null)), new(tuple => Task.CompletedTask, true));
        registry.Add(SpaceTemplate.Create((1, "a")), new(tuple => throw new Exception("Test"), true));

        SpaceTuple tuple = SpaceTuple.Create((1, "a"));
        await callbackChannel.Writer.WriteAsync(tuple);

        int rounds = 0;

        await foreach (ITuple result in continuationChannel.Reader.ReadAllAsync(default))
        {
            Assert.NotNull(result);
            Assert.True(result is SpaceTemplate);
            Assert.True(((SpaceTemplate)result).IsSatisfiedBy(tuple));

            rounds++;

            if (continuationChannel.Reader.Count == 0)
            {
                break;
            }
        }

        Assert.Equal(2, rounds);
        Assert.True(hostStopped);
    }
}