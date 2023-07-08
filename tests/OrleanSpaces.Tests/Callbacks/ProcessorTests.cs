using OrleanSpaces.Callbacks;
using OrleanSpaces.Continuations;
using OrleanSpaces.Tuples;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Tests.Callbacks;

public class SpaceProcessorTests : IClassFixture<SpaceProcessorTests.Fixture>
{
    private readonly CallbackRegistry registry;
    private readonly CallbackChannel<SpaceTuple> callbackChannel;
    private readonly ContinuationChannel<SpaceTuple, SpaceTemplate> continuationChannel;

    public SpaceProcessorTests(Fixture fixture)
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

        continuationChannel.TupleReader.TryRead(out SpaceTuple result);

        Assert.Equal(0, result.Length);
    }

    [Fact]
    public async Task Should_Forward_Templates_When_Tuple_Matches_Them_In_Registry()
    {
        registry.Add(new(1, "a"), new(tuple => Task.CompletedTask, true));
        registry.Add(new(1, "a"), new(tuple => Task.CompletedTask, false));
        registry.Add(new(1, null), new(tuple => Task.CompletedTask, true));
        registry.Add(new(1, null), new(tuple => Task.CompletedTask, false));
        registry.Add(new(1, null), new(tuple => Task.CompletedTask, true));
        registry.Add(new(1, "a", 1.5F), new(tuple => Task.CompletedTask, true));

        SpaceTuple tuple = new(1, "a");
        await callbackChannel.Writer.WriteAsync(tuple);

        int rounds = 3;

        while (rounds > 0)
        {
            if (continuationChannel.TemplateReader.TryRead(out SpaceTemplate result))
            {
                Assert.NotEqual(0, result.Length);
                Assert.True(result.Matches(tuple));

                rounds--;
            }
        }
    }

    public class Fixture : IAsyncLifetime
    {
        private readonly CallbackProcessor processor;

        internal CallbackChannel<SpaceTuple> CallbackChannel { get; }
        internal ContinuationChannel<SpaceTuple, SpaceTemplate> ContinuationChannel { get; }
        internal CallbackRegistry Registry { get; private set; }

        public Fixture()
        {
            CallbackChannel = new();
            ContinuationChannel = new();
            Registry = new();
            processor = new(Registry, CallbackChannel, ContinuationChannel);
        }

        public Task InitializeAsync() => processor.StartAsync(default);
        public Task DisposeAsync() => processor.StopAsync(default);
    }
}

public class IntProcessorTests : IClassFixture<IntProcessorTests.Fixture>
{
    private readonly CallbackRegistry<int, IntTuple, IntTemplate> registry;
    private readonly CallbackChannel<IntTuple> callbackChannel;
    private readonly ContinuationChannel<IntTuple, IntTemplate> continuationChannel;

    public IntProcessorTests(Fixture fixture)
    {
        registry = fixture.Registry;
        callbackChannel = fixture.CallbackChannel;
        continuationChannel = fixture.ContinuationChannel;
    }


    [Fact]
    public async Task Should_Not_Forward_Templates_If_Tuple_Matches_Nothing_In_Registry()
    {
        IntTuple tuple = new(1);
        await callbackChannel.Writer.WriteAsync(tuple);

        continuationChannel.TupleReader.TryRead(out IntTuple result);

        Assert.Equal(0, result.Length);
    }

    [Fact]
    public async Task Should_Forward_Templates_When_Tuple_Matches_Them_In_Registry()
    {
        registry.Add(new(1, 2), new(tuple => Task.CompletedTask, true));
        registry.Add(new(1, 2), new(tuple => Task.CompletedTask, false));
        registry.Add(new(1, null), new(tuple => Task.CompletedTask, true));
        registry.Add(new(1, null), new(tuple => Task.CompletedTask, false));
        registry.Add(new(1, null), new(tuple => Task.CompletedTask, true));
        registry.Add(new(1, 2, 3), new(tuple => Task.CompletedTask, true));

        IntTuple tuple = new(1, 2);
        await callbackChannel.Writer.WriteAsync(tuple);

        int rounds = 3;

        while (rounds > 0)
        {
            if (continuationChannel.TemplateReader.TryRead(out IntTemplate result))
            {
                Assert.NotEqual(0, result.Length);
                Assert.True(result.Matches(tuple));

                rounds--;
            }
        }
    }

    public class Fixture : IAsyncLifetime
    {
        private readonly CallbackProcessor<int, IntTuple, IntTemplate> processor;

        internal CallbackChannel<IntTuple> CallbackChannel { get; }
        internal ContinuationChannel<IntTuple, IntTemplate> ContinuationChannel { get; }
        internal CallbackRegistry<int, IntTuple, IntTemplate> Registry { get; private set; }

        public Fixture()
        {
            CallbackChannel = new();
            ContinuationChannel = new();
            Registry = new();
            processor = new(Registry, CallbackChannel, ContinuationChannel);
        }

        public Task InitializeAsync() => processor.StartAsync(default);
        public Task DisposeAsync() => processor.StopAsync(default);
    }
}