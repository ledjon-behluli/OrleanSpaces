using OrleanSpaces.Evaluations;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Tests.Evaluations;

public class ChannelTests
{
    private readonly EvaluationChannel channel = new();

    [Fact]
    public void Should_Be_An_IConsumable()
    {
        Assert.True(typeof(IConsumable).IsAssignableFrom(typeof(EvaluationChannel)));
    }

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
