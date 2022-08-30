using OrleanSpaces;
using OrleanSpaces.Observers;
using OrleanSpaces.Primitives;

public class Ponger : ISpaceObserver
{
    private readonly SpaceTemplate _template = SpaceTemplate.Create(("Ping", UnitField.Null));
    private readonly ISpaceAgent _client;

    public Ponger(ISpaceAgent client) => _client = client;

    public async Task OnTupleAsync(SpaceTuple tuple)
    {
        if (_template.IsSatisfied(tuple))
        {
            Console.WriteLine("PONG-er: Got it");
            await _client.WriteAsync(SpaceTuple.Create(("Pong", DateTime.Now)));
        }
    }
}