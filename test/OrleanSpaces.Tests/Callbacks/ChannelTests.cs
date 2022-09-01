using OrleanSpaces.Callbacks;
using OrleanSpaces.Primitives;

namespace OrleanSpaces.Tests.Callbacks;

public class ChannelTests
{
    [Fact]
    public async Task Should_Read_What_Was_Writen()
    {
        SpaceTuple tuple = SpaceTuple.Create(1);

        await CallbackChannel.Writer.WriteAsync(tuple);
        SpaceTuple result = await CallbackChannel.Reader.ReadAsync();

        Assert.Equal(tuple, result);
    }
}
