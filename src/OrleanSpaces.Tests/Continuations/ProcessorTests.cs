using OrleanSpaces.Continuations;
using OrleanSpaces.Primitives;

namespace OrleanSpaces.Tests.Continuations;

public class ProcessorTests : IClassFixture<Fixture>
{
    private readonly ContinuationChannel channel;
    private readonly TestTupleRouter router;

    public ProcessorTests(Fixture fixture)
    {
        channel = fixture.Channel;
        router = fixture.Router;
    }

    [Fact]
    public async Task Should_Forward_Tuple_To_Router()
    {
        SpaceTuple tuple = new(1);
        await channel.Writer.WriteAsync(tuple);

        while (router.Tuple == null)
        {

        }

        Assert.Equal(tuple, router.Tuple);

        router.Tuple = null;
    }

    [Fact]
    public async Task Should_Forward_Template_To_Router()
    {
        SpaceTemplate template = new(1);
        await channel.Writer.WriteAsync(template);

        while (router.Tuple == null)
        {

        }

        Assert.Equal(template, router.Tuple);

        router.Tuple = null;
    }
}