using OrleanSpaces;
using OrleanSpaces.Observers;
using OrleanSpaces.Primitives;

public class Ponger : ISpaceObserver
{
    private readonly SpaceTemplate template;
    private readonly ISpaceChannel channel;

    public Ponger(ISpaceChannel channel)
    {
        this.channel = channel;
        template = SpaceTemplate.Create(("Ping", UnitField.Null));
    }

    public async Task OnTupleAsync(SpaceTuple tuple)
    {
        if (template.IsSatisfied(tuple))
        {
            Console.WriteLine("PONG-er: Got it");

            var agent = await channel.GetAsync();
            await agent.WriteAsync(SpaceTuple.Create(("Pong", DateTime.Now)));
        }
    }
}