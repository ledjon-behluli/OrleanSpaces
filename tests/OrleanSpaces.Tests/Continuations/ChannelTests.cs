using OrleanSpaces.Continuations;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Tests.Continuations;

public class ChannelTests
{
    private readonly ContinuationChannel channel = new();

    [Fact]
    public void Should_Be_An_IConsumable()
    {
        Assert.True(typeof(IConsumable).IsAssignableFrom(typeof(ContinuationChannel)));
    }

    [Fact]
    public async Task Should_Read_Tuple_If_Tuple_Was_Writen()
    {
        SpaceTuple tuple = new(1);

        await channel.Writer.WriteAsync(tuple);
        ISpaceTuple result = await channel.Reader.ReadAsync();

        Assert.Equal(tuple, result);
        Assert.Equal(typeof(SpaceTuple), result.GetType());
        Assert.NotEqual(typeof(SpaceTemplate), result.GetType());
    }

    [Fact]
    public async Task Should_Read_Template_If_Template_Was_Writen()
    {
        SpaceTemplate template = new(1);

        await channel.Writer.WriteAsync(template);
        ISpaceTuple result = await channel.Reader.ReadAsync();

        Assert.Equal(template, result);
        Assert.Equal(typeof(SpaceTemplate), result.GetType());
        Assert.NotEqual(typeof(SpaceTuple), result.GetType());
    }
}
