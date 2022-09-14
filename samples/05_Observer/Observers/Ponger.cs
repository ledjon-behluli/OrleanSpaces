using OrleanSpaces;
using OrleanSpaces.Observers;
using OrleanSpaces.Primitives;
using System;

public class Ponger : SpaceObserver
{
    private readonly SpaceTemplate template;
    private readonly ISpaceChannel channel;

    public Ponger(ISpaceChannel channel)
    {
        Observe(SpaceEvent.TupleAdded);

        this.channel = channel;
        template = new(("Ping", SpaceUnit.Null));
    }

    public override async Task OnAddedAsync(SpaceTuple tuple, CancellationToken cancellationToken)
    {
        if (template.IsSatisfiedBy(tuple))
        {
            Console.WriteLine("PONG-er: Got it");

            var agent = await channel.GetAsync();
            await agent.WriteAsync(new(("Pong", DateTime.Now)));
        }
    }
}
