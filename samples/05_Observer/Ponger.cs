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
        template = new(("Ping", SpaceUnit.Null));
    }

    public async Task OnAddedAsync(SpaceTuple tuple, CancellationToken cancellationToken)
    {
        if (template.IsSatisfiedBy(tuple))
        {
            Console.WriteLine("PONG-er: Got it");

            var agent = await channel.GetAsync();
            await agent.WriteAsync(new(("Pong", DateTime.Now)));
        }
    }

    public async Task OnRemovedAsync(SpaceTemplate, CancellationToken cancellationToken)
    {
        // TODO: Fill me!
        throw new NotImplementedException();
    }

    public Task OnEmptyAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("PONG-er: Got info that space is empty");
        return Task.CompletedTask;
    }
}