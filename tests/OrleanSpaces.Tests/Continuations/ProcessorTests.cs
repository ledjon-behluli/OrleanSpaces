using OrleanSpaces.Continuations;
using OrleanSpaces.Tuples;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Tests.Continuations;

public class SpaceProcessorTests : IClassFixture<SpaceProcessorTests.Fixture>
{
    private readonly ContinuationChannel<SpaceTuple, SpaceTemplate> channel;
    private readonly TestTupleRouter<SpaceTuple, SpaceTemplate> router;

    public SpaceProcessorTests(Fixture fixture)
    {
        channel = fixture.Channel;
        router = fixture.Router;
    }

    [Fact]
    public async Task Should_Forward_Tuple_To_Router()
    {
        SpaceTuple tuple = new(1);
        await channel.TupleWriter.WriteAsync(tuple);

        while (router.Tuple.Length == 0)
        {

        }

        Assert.Equal(tuple, router.Tuple);
        router.Tuple = new();
    }

    [Fact]
    public async Task Should_Forward_Template_To_Router()
    {
        SpaceTemplate template = new(1);
        await channel.TemplateWriter.WriteAsync(template);

        while (router.Template.Length == 0)
        {

        }

        Assert.Equal(template, router.Template);
        router.Template = template;
    }

    public class Fixture : IAsyncLifetime
    {
        private readonly ContinuationProcessor<SpaceTuple, SpaceTemplate> processor;

        internal TestTupleRouter<SpaceTuple, SpaceTemplate> Router { get; }
        internal ContinuationChannel<SpaceTuple, SpaceTemplate> Channel { get; }

        public Fixture()
        {
            Channel = new();
            Router = new TestTupleRouter<SpaceTuple, SpaceTemplate>();

            processor = new(Channel, Router);
        }

        public Task InitializeAsync() => processor.StartAsync(default);
        public Task DisposeAsync() => processor.StopAsync(default);
    }
}

public class IntProcessorTests : IClassFixture<IntProcessorTests.Fixture>
{
    private readonly ContinuationChannel<IntTuple, IntTemplate> channel;
    private readonly TestTupleRouter<IntTuple, IntTemplate> router;

    public IntProcessorTests(Fixture fixture)
    {
        channel = fixture.Channel;
        router = fixture.Router;
    }

    [Fact]
    public async Task Should_Forward_Tuple_To_Router()
    {
        IntTuple tuple = new(1);
        await channel.TupleWriter.WriteAsync(tuple);

        while (router.Tuple.Length == 0)
        {

        }

        Assert.Equal(tuple, router.Tuple);
        router.Tuple = new();
    }

    [Fact]
    public async Task Should_Forward_Template_To_Router()
    {
        IntTemplate template = new(1);
        await channel.TemplateWriter.WriteAsync(template);

        while (router.Template.Length == 0)
        {

        }

        Assert.Equal(template, router.Template);
        router.Template = template;
    }

    public class Fixture : IAsyncLifetime
    {
        private readonly ContinuationProcessor<IntTuple, IntTemplate> processor;

        internal TestTupleRouter<IntTuple, IntTemplate> Router { get; }
        internal ContinuationChannel<IntTuple, IntTemplate> Channel { get; }

        public Fixture()
        {
            Channel = new();
            Router = new TestTupleRouter<IntTuple, IntTemplate>();

            processor = new(Channel, Router);
        }

        public Task InitializeAsync() => processor.StartAsync(default);
        public Task DisposeAsync() => processor.StopAsync(default);
    }
}