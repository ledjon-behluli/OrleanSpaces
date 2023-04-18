using OrleanSpaces.Observers;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Tests.Observers;

public class ChannelTests
{
    private readonly ObserverChannel channel = new();

    [Fact]
    public void Should_Be_An_IConsumable()
    {
        Assert.True(typeof(IConsumable).IsAssignableFrom(typeof(ObserverChannel)));
    }

    [Fact]
    public async Task Should_Read_What_Was_Writen()
    {
        SpaceTuple tuple = new(1);

        await channel.Writer.WriteAsync(tuple);
        ISpaceTuple result = await channel.Reader.ReadAsync();

        Assert.Equal(tuple, result);
    }
}
