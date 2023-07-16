using OrleanSpaces;
using OrleanSpaces.Tuples;

public class Ponger : SpaceObserver<SpaceTuple>
{
    private readonly SpaceTemplate template;
    private readonly ISpaceAgent agent;

    public Ponger(ISpaceAgent agent)
    {
        ListenTo(EventType.Expansions);

        this.agent = agent;
        template = new("ping", null);
    }

    public override async Task OnExpansionAsync(SpaceTuple tuple, CancellationToken cancellationToken)
    {
        if (template.Matches(tuple))
        {
            Console.WriteLine("PONG-er: Got it");
            await agent.WriteAsync(new("pong", DateTime.Now));
        }
    }

    public override Task OnContractionAsync(SpaceTuple tuple, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public override Task OnFlatteningAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
