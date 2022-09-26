using OrleanSpaces;
using OrleanSpaces.Observers;
using OrleanSpaces.Tuples;

public class Ponger : SpaceObserver
{
    private readonly SpaceTemplate template;
    private readonly ISpaceAgentProvider provider;

    public Ponger(ISpaceAgentProvider provider)
    {
        ListenTo(EventType.Expansions);

        this.provider = provider;
        template = new("Ping", SpaceUnit.Null);
    }

    public override async Task OnExpansionAsync(SpaceTuple tuple, CancellationToken cancellationToken)
    {
        if (template.Matches(tuple))
        {
            Console.WriteLine("PONG-er: Got it");

            var agent = await provider.GetAsync();
            await agent.WriteAsync(new("Pong", DateTime.Now));
        }
    }
}
