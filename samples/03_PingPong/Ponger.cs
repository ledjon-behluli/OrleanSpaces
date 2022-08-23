using OrleanSpaces.Clients;
using OrleanSpaces.Core.Observers;
using OrleanSpaces.Core.Primitives;

public class Ponger : ISpaceObserver
{
    private readonly ISpaceClient _client;
    private readonly SpaceTemplate _template;

    public Ponger(ISpaceClient client)
    {
        _client = client;
        _template = SpaceTemplate.Create(("Ping", UnitField.Null));
    }

    public async Task PongAsync()
    {
        SpaceTuple tuple = SpaceTuple.Create(("Pong", DateTime.Now));
        await _client.WriteAsync(tuple);
        Console.WriteLine($"PONG-er: Sent out '{tuple}'");
    }

    public void Receive(SpaceTuple tuple)
    {
        if (_template.IsSatisfied(tuple))
        {
            Console.WriteLine($"PONG-er: Yep this is it '{tuple}'");
        }
        else
        {
            Console.WriteLine($"PONG-er: Not what I need '{tuple}'");
        }
    }
}