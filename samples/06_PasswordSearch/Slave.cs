using OrleanSpaces;
using OrleanSpaces.Observers;
using OrleanSpaces.Primitives;

public class Slave : SpaceObserver
{
    private readonly int id; 
    private readonly ISpaceAgent agent;
    private readonly SpaceTemplate template;
    private readonly Dictionary<string, string> hashPasswordPairs;

    public Slave(int id, ISpaceAgent agent, Dictionary<string, string> hashPasswordPairs)
    {
        ListenTo(Expansions);

        this.id = id;
        this.agent = agent;
        this.template = new((ExchangeKeys.PASSWORD_SEARCH, typeof(string)));
        this.hashPasswordPairs = hashPasswordPairs;
    }

    public override async Task OnExpansionAsync(SpaceTuple tuple, CancellationToken cancellationToken)
    {
        if (template.IsSatisfiedBy(tuple))
        {
            string hash = (string)tuple[1];

#nullable disable
            if (hashPasswordPairs.TryGetValue(hash, out string password))
#nullable enable
            {
                Console.WriteLine($"\nSLAVE {id}: Found password to Hash = {hash}");

                await Task.Factory.StartNew(async () =>
                {
                    await agent.WriteAsync(new((ExchangeKeys.PASSWORD_FOUND, hash, password)));
                    await agent.PopAsync(tuple);

                    await Task.Delay(100, cancellationToken); // Simulating some complex hash-pw search process.
                });
            }
        }
    }
}