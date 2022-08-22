using OrleanSpaces.Clients;
using OrleanSpaces.Core.Observers;
using OrleanSpaces.Core.Primitives;

namespace PingPong;

public class Constants
{
	public const string ChannelKey = "pp-channel";
}

public class Pinger : ISpaceObserver
{
	private readonly ISpaceClient client;
	private readonly SpaceTuple ppTuple;

	public Pinger(ISpaceClient client)
	{
		this.client = client;
		//this.ppTuple = new SpaceTuple();
	}

	public async Task PingAsync() =>
		await client.WriteAsync(SpaceTuple.Create((Constants.ChannelKey, "Ping")));

	public void Receive(SpaceTuple tuple)
	{
		//if (tuple )
	}
}