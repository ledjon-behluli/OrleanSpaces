using Orleans;
using OrleanSpaces.Core;
using OrleanSpaces.Core.Primitives;

namespace OrleanSpaces.Clients;

public class BlockingReaderClient
{
    protected IGrainFactory Factory { get; }

	public BlockingReaderClient(IGrainFactory factory)
	{
		Factory = factory;
	}

	public async Task Test()
	{
		var reader = Factory.GetSpaceBlockingReader();

		await reader.PeekAsync(SpaceTemplate.Create(1), x => Task.CompletedTask);
	}
}

public class Tests
{
	[Fact]
	public void A()
	{

	}
}