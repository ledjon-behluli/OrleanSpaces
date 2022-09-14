using OrleanSpaces;
using OrleanSpaces.Observers;
using OrleanSpaces.Primitives;

public class Ponger : DynamicObserver
{
    private readonly SpaceTemplate template;
    private readonly ISpaceChannel channel;

    public Ponger(ISpaceChannel channel)
    {
        Interested(In.Expansions);

        this.channel = channel;
        template = new(("Ping", SpaceUnit.Null));
    }

    public override async Task OnExpansionAsync(SpaceTuple tuple, CancellationToken cancellationToken)
    {
        if (template.IsSatisfiedBy(tuple))
        {
            Console.WriteLine("PONG-er: Got it");

            var agent = await channel.GetAsync();
            await agent.WriteAsync(new(("Pong", DateTime.Now)));
        }
    }
}
