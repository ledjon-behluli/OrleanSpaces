using OrleanSpaces.Clients;
using OrleanSpaces.Core.Observers;
using OrleanSpaces.Core.Primitives;

public static class Constants
{
    public static string EXCHANGE_KEY = "pp-channel";
}

public class Pinger : ISpaceObserver
{
	private readonly ISpaceClient _client;
    private readonly SpaceTemplate _template = SpaceTemplate.Create(("Pong", UnitField.Null));

    private int count;
    public bool IsDone => count == 3;

    public Pinger(ISpaceClient client) => _client = client;

    public async Task ReceiveAsync(SpaceTuple tuple)
	{
        if (!IsDone && _template.IsSatisfied(tuple))
        {
            Console.WriteLine("PING-er: Received = {tuple}");

            //await Task.Delay(500);
            var _tuple = SpaceTuple.Create(("Ping", DateTime.Now));
            await _client.WriteAsync(_tuple);

            Console.WriteLine($"PING-er: Wrote back = {_tuple}");
            
            count++;
		}
    }
}
