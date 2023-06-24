using OrleanSpaces;
using OrleanSpaces.Observers;
using OrleanSpaces.Tuples;

public class Ponger : SpaceObserver<SpaceTuple>
{
    private readonly SpaceTemplate template;
    private readonly ISpaceAgentProvider provider;

    public Ponger(ISpaceAgentProvider provider)
    {
        ListenTo(EventType.Expansions);

        this.provider = provider;
        template = new("ping", null);
    }

    public override async Task OnExpansionAsync(SpaceTuple tuple, CancellationToken cancellationToken)
    {
        if (template.Matches(tuple))
        {
            Console.WriteLine("PONG-er: Got it");

            var agent = await provider.GetAsync();
            await agent.WriteAsync(new("pong", DateTime.Now));
        }
    }
}
