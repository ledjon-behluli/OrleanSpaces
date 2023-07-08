using OrleanSpaces.Observers;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Tests.Observers;

public class ChannelTests
{
    private readonly ObserverChannel<SpaceTuple> channel = new();

    [Fact]
    public async Task Should_Read_What_Was_Writen()
    {
        TupleAction<SpaceTuple> action = new(Guid.NewGuid(), new(1), TupleActionType.Insert);

        await channel.Writer.WriteAsync(action);
        TupleAction<SpaceTuple> result = await channel.Reader.ReadAsync();

        Assert.Equal(action, result);
    }
}
