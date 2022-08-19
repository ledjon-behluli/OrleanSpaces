using Orleans;
using OrleanSpaces.Core;
using OrleanSpaces.Core.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrleanSpaces.Clients;

public abstract class BlockingReaderClient
{
    protected IGrainFactory Factory { get; }

	public BlockingReaderClient(IGrainFactory factory)
	{
		Factory = factory;
	}

	public async Task Test()
	{
		var reader = Factory.GetSpaceBlockingReader();
		await reader.PeekAsync(SpaceTemplate.Create(1), tuple =>
		{

		});
	}
}
