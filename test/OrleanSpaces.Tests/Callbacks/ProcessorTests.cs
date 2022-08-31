using OrleanSpaces.Callbacks;
using OrleanSpaces.Primitives;

namespace OrleanSpaces.Tests.Callbacks;

public class ProcessorTests : IClassFixture<ProcessorFixture>
{
    [Fact]
    public async void Should_Not_Forward_If_Tuple_Matches_Nothing_In_Registry()
    {
        SpaceTuple tuple = SpaceTuple.Create(1);
        await CallbackChannel.Writer.WriteAsync(tuple);


    }
}
