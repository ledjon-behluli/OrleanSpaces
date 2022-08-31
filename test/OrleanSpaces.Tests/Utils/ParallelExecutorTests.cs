using OrleanSpaces.Utils;

namespace OrleanSpaces.Tests.Utils;

public class ParallelExecutorTests
{
    [Fact]
    public async Task Should_Execute()
    {
        List<Callable> callables = new() { new(), new(), new() };

        await ParallelExecutor.WhenAll(callables, async x => await x.CallMe());

        Assert.All(callables, x => Assert.True(x.WasCalled));
    }

    private class Callable
    {
        public bool WasCalled { get; private set; }

        public async Task CallMe()
        {
            await Task.Delay(1);
            WasCalled = true;
        }
    }
}
