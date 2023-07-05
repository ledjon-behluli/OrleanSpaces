using OrleanSpaces;
using OrleanSpaces.Observers;
using OrleanSpaces.Tuples;

public class Master : SpaceObserver<SpaceTuple>
{
    private readonly ISpaceAgent agent;
    private readonly SpaceTemplate template;
    private readonly Dictionary<string, string> hashPasswordPairs;
    
    public bool IsDone { get; private set; }
    public IReadOnlyDictionary<string, string> HashPasswordPairs => hashPasswordPairs;

    public Master(ISpaceAgent agent, int take, List<string> hashes)
    {
        ListenTo(Expansions);

        this.agent = agent;
        this.template = new(ExchangeKeys.PASSWORD_FOUND, typeof(string), typeof(string));
        this.hashPasswordPairs = new();

        Random rand = new();
        while (take > 0)
        {
            string randomHash = hashes[rand.Next(hashes.Count)];

            if (!hashPasswordPairs.ContainsKey(randomHash))
            {
                hashPasswordPairs.Add(randomHash, string.Empty);
                take--;
            }
        }
    }

    public async Task RunAsync()
    {
        foreach (var hash in hashPasswordPairs.Keys)
        {
            await agent.WriteAsync(new(ExchangeKeys.PASSWORD_SEARCH, hash));
        }
    }

    public override async Task OnExpansionAsync(SpaceTuple tuple, CancellationToken cancellationToken)
    {
        if (template.Matches(tuple))
        {
            string hash = (string)tuple[1];

            Console.WriteLine($"MASTER: Received solution to Hash = {hash}");
            await agent.PopAsync((SpaceTemplate)tuple.ToTemplate());

            string password = (string)tuple[2];
            hashPasswordPairs[hash] = password;

            if (hashPasswordPairs.All(x => x.Value.Length > 0))
            {
                IsDone = true;
            }
        }
    }
}
