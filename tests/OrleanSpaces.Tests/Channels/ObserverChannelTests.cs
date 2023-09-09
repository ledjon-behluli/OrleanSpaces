using OrleanSpaces.Channels;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Tests.Channels;

public class ObserverChannelTests
{
    private readonly ObserverChannel<SpaceTuple> channel = new();

    [Fact]
    public async Task Should_Read_What_Was_Writen()
    {
        TupleAction<SpaceTuple> action = new(Guid.NewGuid(), new SpaceTuple(1).WithDefaultStore(), TupleActionType.Insert);

        await channel.Writer.WriteAsync(action);
        TupleAction<SpaceTuple> result = await channel.Reader.ReadAsync();

        Assert.Equal(action, result);
    }
}
