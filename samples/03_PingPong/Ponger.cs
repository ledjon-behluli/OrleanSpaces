using OrleanSpaces.Clients;
using OrleanSpaces.Core.Observers;
using OrleanSpaces.Core.Primitives;

public class Ponger : ISpaceObserver
{
    private readonly SpaceTemplate _template = SpaceTemplate.Create(("Ping", UnitField.Null));
    private readonly ISpaceClient _client;

    public Ponger(ISpaceClient client) => _client = client;

    public async Task ReceiveAsync(SpaceTuple tuple)
    {
        if (_template.IsSatisfied(tuple))
        {
            Console.WriteLine("PONG-er: Got it");
            await _client.WriteAsync(SpaceTuple.Create(("Pong", DateTime.Now)));
        }
    }
}