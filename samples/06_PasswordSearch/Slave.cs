using OrleanSpaces;
using OrleanSpaces.Primitives;

public class Slave
{
    private readonly int id; 
    private readonly ISpaceAgent agent;
    private readonly SpaceTemplate template;
    private readonly Dictionary<string, string> hashPasswordPairs;

    public Slave(int id, ISpaceAgent agent, Dictionary<string, string> hashPasswordPairs)
    {
        this.id = id;
        this.agent = agent;
        this.template = new((ExchangeKeys.PASSWORD_SEARCH, typeof(string)));
        this.hashPasswordPairs = hashPasswordPairs;
    }

    public async Task RunAsync()
    {
        IEnumerable<SpaceTuple> tuples = await agent.ScanAsync(template);
        foreach (var tuple in tuples)
        {
            await Task.Delay(100);  // Simulate some complex searching.
            string hash = (string)tuple[1];

            if (hashPasswordPairs.TryGetValue(hash, out string? password))
            {
                Console.WriteLine($"\nSLAVE {id}: Found password to Hash = {hash}");
                await agent.WriteAsync(new((ExchangeKeys.PASSWORD_FOUND, hash, password)));
            }
        }
    }
}