using OrleanSpaces.Observers;
using OrleanSpaces.Primitives;

namespace OrleanSpaces.Tests.Observers;

public class ChannelTests
{
    [Fact]
    public async Task Should_Read_What_Was_Writen()
    {
        SpaceTuple tuple = SpaceTuple.Create(1);

        await ObserverChannel.Writer.WriteAsync(tuple);
        SpaceTuple result = await ObserverChannel.Reader.ReadAsync();

        Assert.Equal(tuple, result);
    }
}
