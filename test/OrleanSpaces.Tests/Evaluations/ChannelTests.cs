using OrleanSpaces.Evaluations;
using OrleanSpaces.Primitives;

namespace OrleanSpaces.Tests.Evaluations;

public class ChannelTests
{
    [Fact]
    public async Task Should_Read_What_Was_Writen()
    {
        SpaceTuple tuple = SpaceTuple.Create(1);
        Func<Task<SpaceTuple>> evaluation = () => Task.FromResult(tuple);

        await EvaluationChannel.Writer.WriteAsync(evaluation);
        Func<Task<SpaceTuple>> result = await EvaluationChannel.Reader.ReadAsync();

        Assert.Equal(evaluation, result);
        Assert.Equal(tuple, await evaluation());
    }
}
