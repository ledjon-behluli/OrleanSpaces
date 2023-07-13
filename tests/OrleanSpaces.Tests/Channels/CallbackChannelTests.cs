using OrleanSpaces.Channels;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Tests.Channels;

public class CallbackChannelTests
{
    private readonly CallbackChannel<SpaceTuple> channel = new();

    [Fact]
    public async Task Should_Read_What_Was_Writen()
    {
        SpaceTuple tuple = new(1);

        await channel.Writer.WriteAsync(tuple);
        SpaceTuple result = await channel.Reader.ReadAsync();

        Assert.Equal(tuple, result);
    }
}