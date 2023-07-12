using OrleanSpaces.Channels;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Tests.Continuations;

public class ChannelTests
{
    private readonly ContinuationChannel<SpaceTuple, SpaceTemplate> channel = new();

    [Fact]
    public async Task Should_Read_Tuple_If_Tuple_Was_Writen()
    {
        SpaceTuple tuple = new(1);

        await channel.TupleWriter.WriteAsync(tuple);
        ISpaceTuple result = await channel.TupleReader.ReadAsync();

        Assert.Equal(tuple, result);
        Assert.Equal(typeof(SpaceTuple), result.GetType());
        Assert.NotEqual(typeof(SpaceTemplate), result.GetType());
    }

    [Fact]
    public async Task Should_Read_Template_If_Template_Was_Writen()
    {
        SpaceTemplate template = new(1);

        await channel.TemplateWriter.WriteAsync(template);
        ISpaceTemplate result = await channel.TemplateReader.ReadAsync();

        Assert.Equal(template, result);
        Assert.Equal(typeof(SpaceTemplate), result.GetType());
        Assert.NotEqual(typeof(SpaceTuple), result.GetType());
    }
}
