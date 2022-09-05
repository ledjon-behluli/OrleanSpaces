using OrleanSpaces.Continuations;
using OrleanSpaces.Primitives;

namespace OrleanSpaces.Tests.Continuations;

public class ProcessorTests : IClassFixture<Fixture>
{
    private readonly ContinuationChannel channel;
    private readonly TestRouter router;

    public ProcessorTests(Fixture fixture)
    {
        channel = fixture.Channel;
        router = fixture.Router;
    }

    [Fact]
    public async Task Should_Forward_Tuple_To_Router()
    {
        SpaceTuple tuple = SpaceTuple.Create("continue");
        await channel.Writer.WriteAsync(tuple);

        ISpaceElement element = null;

        do
        {
            element = router.Elements.SingleOrDefault(x => x.Equals(tuple));
        }
        while (element == null);

        Assert.Equal(tuple, element);
    }

    [Fact]
    public async Task Should_Forward_Template_To_Router()
    {
        SpaceTemplate template = SpaceTemplate.Create("continue");
        await channel.Writer.WriteAsync(template);

        ISpaceElement element = null;

        do
        {
            element = router.Elements.SingleOrDefault(x => x.Equals(template));
        }
        while (element == null);

        Assert.Equal(template, element);
    }
}