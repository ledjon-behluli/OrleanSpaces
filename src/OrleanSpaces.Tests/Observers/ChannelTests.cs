using OrleanSpaces.Observers;
using OrleanSpaces.Primitives;

namespace OrleanSpaces.Tests.Observers;

public class ChannelTests
{
    private readonly ObserverChannel channel = new();

    [Fact]
    public async Task Should_Read_What_Was_Writen()
    {
        SpaceTuple tuple = SpaceTuple.Create(1);

        await channel.Writer.WriteAsync(tuple);
        SpaceTuple result = await channel.Reader.ReadAsync();

        Assert.Equal(tuple, result);
    }
}
