using OrleanSpaces;
using OrleanSpaces.Observers;
using OrleanSpaces.Primitives;

public class Master : SpaceObserver
{
    private readonly ISpaceAgent agent;
    private readonly SpaceTemplate template;

    public bool IsDone { get; private set; }
    public Dictionary<string, string> HashPasswordPairs { get; private set; }

    public Master(ISpaceAgent agent, int take, List<string> hashes)
    {
        ListenTo(Expansions);

        this.agent = agent;
        this.template = new((ExchangeKeys.PASSWORD_FOUND, typeof(string), typeof(string)));
        this.HashPasswordPairs = new();

        Random rand = new();

        while (take > 0)
        {
            string randomHash = hashes[rand.Next(hashes.Count)];

            if (!HashPasswordPairs.ContainsKey(randomHash))
            {
                HashPasswordPairs.Add(randomHash, string.Empty);
                take--;
            }
        }
    }

    public async Task RunAsync()
    {
        foreach (var hash in HashPasswordPairs.Keys)
        {
            await agent.WriteAsync(new((ExchangeKeys.PASSWORD_SEARCH, hash)));
        }
    }

    public override Task OnExpansionAsync(SpaceTuple tuple, CancellationToken cancellationToken)
    {
        if (template.IsSatisfiedBy(tuple))
        {
            Console.WriteLine($"MASTER: Received solution {tuple}");

            string hash = (string)tuple[1];
            string password = (string)tuple[2];

            HashPasswordPairs[hash] = password;
            if (HashPasswordPairs.All(x => x.Value.Length > 0))
            {
                IsDone = true;
            }
        }

        return Task.CompletedTask;
    }
}
