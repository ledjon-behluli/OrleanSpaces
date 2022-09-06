using OrleanSpaces.Callbacks;
using OrleanSpaces.Continuations;
using OrleanSpaces.Primitives;

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
    public async void Should_Not_Forward_Templates_If_Tuple_Matches_Nothing_In_Registry()
    {
        SpaceTuple tuple = SpaceTuple.Create(1);
        await callbackChannel.Writer.WriteAsync(tuple);

        continuationChannel.Reader.TryRead(out ISpaceElement element);

        Assert.Null(element);
    }

    [Fact]
    public async void Should_Forward_Templates_When_Tuple_Matches_Them_In_Registry()
    {
        registry.Add(SpaceTemplate.Create((1, "a")), new(tuple => Task.CompletedTask, true));
        registry.Add(SpaceTemplate.Create((1, "a")), new(tuple => Task.CompletedTask, false));
        registry.Add(SpaceTemplate.Create((1, UnitField.Null)), new(tuple => Task.CompletedTask, true));
        registry.Add(SpaceTemplate.Create((1, UnitField.Null)), new(tuple => Task.CompletedTask, false));
        registry.Add(SpaceTemplate.Create((1, UnitField.Null)), new(tuple => Task.CompletedTask, true));
        registry.Add(SpaceTemplate.Create((1, "a", 1.5F)), new(tuple => Task.CompletedTask, true));

        SpaceTuple tuple = SpaceTuple.Create((1, "a"));
        await callbackChannel.Writer.WriteAsync(tuple);

        int rounds = 0;

        await foreach (var element in continuationChannel.Reader.ReadAllAsync(default))
        {
            Assert.NotNull(element);
            Assert.True(element is SpaceTemplate);
            Assert.True(((SpaceTemplate)element).IsSatisfiedBy(tuple));

            rounds++;

            if (continuationChannel.Reader.Count == 0)
            {
                break;
            }
        }

        Assert.Equal(3, rounds);
    }

    [Fact]
    public async Task Should_Continue_Forwarding_If_Any_Callback_Throws()
    {
        registry.Add(SpaceTemplate.Create((1, "a")), new(tuple => Task.CompletedTask, true));
        registry.Add(SpaceTemplate.Create((1, "a")), new(tuple => throw new Exception("Test"), true));
        registry.Add(SpaceTemplate.Create((1, UnitField.Null)), new(tuple => Task.CompletedTask, true));

        SpaceTuple tuple = SpaceTuple.Create((1, "a"));
        await callbackChannel.Writer.WriteAsync(tuple);

        int rounds = 0;

        await foreach (var element in continuationChannel.Reader.ReadAllAsync(default))
        {
            Assert.NotNull(element);
            Assert.True(element is SpaceTemplate);
            Assert.True(((SpaceTemplate)element).IsSatisfiedBy(tuple));

            rounds++;

            if (continuationChannel.Reader.Count == 0)
            {
                break;
            }
        }

        Assert.Equal(2, rounds);
    }
}