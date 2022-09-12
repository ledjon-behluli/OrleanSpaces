using OrleanSpaces.Evaluations;
using OrleanSpaces.Primitives;

namespace OrleanSpaces.Tests.Evaluations;

public class ChannelTests
{
    private readonly EvaluationChannel channel = new();

    [Fact]
    public async Task Should_Read_What_Was_Writen()
    {
        SpaceTuple tuple = new(1);
        Func<Task<SpaceTuple>> evaluation = () => Task.FromResult(tuple);

        await channel.Writer.WriteAsync(evaluation);
        Func<Task<SpaceTuple>> result = await channel.Reader.ReadAsync();

        Assert.Equal(evaluation, result);
        Assert.Equal(tuple, await evaluation());
    }
}
