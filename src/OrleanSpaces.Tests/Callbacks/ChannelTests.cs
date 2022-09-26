using OrleanSpaces.Callbacks;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Tests.Callbacks;

public class ChannelTests
{
    private readonly CallbackChannel channel = new();

    [Fact]
    public void Should_Be_An_IConsumable()
    {
        Assert.True(typeof(IConsumable).IsAssignableFrom(typeof(CallbackChannel)));
    }

    [Fact]
    public async Task Should_Read_What_Was_Writen()
    {
        SpaceTuple tuple = new(1);

        await channel.Writer.WriteAsync(tuple);
        SpaceTuple result = await channel.Reader.ReadAsync();

        Assert.Equal(tuple, result);
    }
}
