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
            string hash = (string)tuple[1];
            string? password = await SearchPasswordAsync(hash);

            if (!string.IsNullOrEmpty(password))
            {
                Console.WriteLine($"\nSLAVE {id}: Found password to Hash = {hash}");

                await agent.PopAsync(tuple);
                await agent.WriteAsync(new((ExchangeKeys.PASSWORD_FOUND, hash, password)));
            }
        }

        async Task<string?> SearchPasswordAsync(string hash)
        {
            await Task.Delay(10);   // Simulate some time to search.
            return hashPasswordPairs.GetValueOrDefault(hash);
        }
    }
}