using OrleanSpaces.Clients;
using OrleanSpaces.Core.Observers;
using OrleanSpaces.Core.Primitives;

public class Ponger : ISpaceObserver
{
    private readonly ISpaceClient _client;
    private readonly SpaceTemplate _template = SpaceTemplate.Create((Constants.EXCHANGE_KEY, "Ping"));

    public int Iterations { get; private set; }

    public Ponger(ISpaceClient client) => _client = client;

    public async Task ReceiveAsync(SpaceTuple tuple)
    {
        if (_template.IsSatisfied(tuple))
        {
            Console.WriteLine($"PONG-er: Received = {tuple}");

            await Task.Delay(500);
            var _tuple = SpaceTuple.Create((Constants.EXCHANGE_KEY, "Pong"));
            await _client.WriteAsync(_tuple);

            Console.WriteLine($"PONG-er: Wrote back = {_tuple}\n");

            Iterations++;
        }

        Console.WriteLine("PONG-er: Not what i am looking");
    }
}