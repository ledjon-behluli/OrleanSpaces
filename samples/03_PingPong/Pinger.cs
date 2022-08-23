using OrleanSpaces.Clients;
using OrleanSpaces.Core.Observers;
using OrleanSpaces.Core.Primitives;

public class Pinger : ISpaceObserver
{
	private readonly ISpaceClient _client;
	private readonly SpaceTemplate _template;

	public Pinger(ISpaceClient client)
	{
		_client = client;
		_template = SpaceTemplate.Create(("Pong", UnitField.Null));
	}

    public async Task PingAsync()
    {
        SpaceTuple tuple = SpaceTuple.Create(("Ping", DateTime.Now));
        await _client.WriteAsync(tuple);
        Console.WriteLine($"PING-er: Sent out '{tuple}'");
    }

	public void Receive(SpaceTuple tuple)
	{
		if (_template.IsSatisfied(tuple))
        {
            Console.WriteLine($"PING-er: Yep this is it '{tuple}'");
        }
        else
        {
            Console.WriteLine($"PING-er: Not what I need '{tuple}'");
        }
	}
}
