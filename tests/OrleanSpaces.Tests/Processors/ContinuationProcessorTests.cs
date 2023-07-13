using OrleanSpaces.Channels;
using OrleanSpaces.Processors;
using OrleanSpaces.Tuples;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Tests.Processors;

public class ContinuationSpaceProcessorTests : IClassFixture<ContinuationSpaceProcessorTests.Fixture>
{
    private readonly ContinuationChannel<SpaceTuple, SpaceTemplate> channel;
    private readonly TestSpaceRouter<SpaceTuple, SpaceTemplate> router;

    public ContinuationSpaceProcessorTests(Fixture fixture)
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

        internal TestSpaceRouter<SpaceTuple, SpaceTemplate> Router { get; }
        internal ContinuationChannel<SpaceTuple, SpaceTemplate> Channel { get; }

        public Fixture()
        {
            Channel = new();
            Router = new TestSpaceRouter<SpaceTuple, SpaceTemplate>();

            processor = new(Channel, Router);
        }

        public Task InitializeAsync() => processor.StartAsync(default);
        public Task DisposeAsync() => processor.StopAsync(default);
    }
}

public class ContinuationIntProcessorTests : IClassFixture<ContinuationIntProcessorTests.Fixture>
{
    private readonly ContinuationChannel<IntTuple, IntTemplate> channel;
    private readonly TestSpaceRouter<IntTuple, IntTemplate> router;

    public ContinuationIntProcessorTests(Fixture fixture)
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

        internal TestSpaceRouter<IntTuple, IntTemplate> Router { get; }
        internal ContinuationChannel<IntTuple, IntTemplate> Channel { get; }

        public Fixture()
        {
            Channel = new();
            Router = new TestSpaceRouter<IntTuple, IntTemplate>();

            processor = new(Channel, Router);
        }

        public Task InitializeAsync() => processor.StartAsync(default);
        public Task DisposeAsync() => processor.StopAsync(default);
    }
}