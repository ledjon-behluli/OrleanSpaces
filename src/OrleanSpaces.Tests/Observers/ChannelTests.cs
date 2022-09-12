using OrleanSpaces.Observers;
using OrleanSpaces.Primitives;
using System.Runtime.CompilerServices;

namespace OrleanSpaces.Tests.Observers;

public class ChannelTests
{
    private readonly ObserverChannel channel = new();

    [Fact]
    public async Task Should_Read_What_Was_Writen()
    {
        SpaceTuple tuple = new(1);

        await channel.Writer.WriteAsync(tuple);
        ITuple result = await channel.Reader.ReadAsync();

        Assert.Equal(tuple, result);
    }
}
