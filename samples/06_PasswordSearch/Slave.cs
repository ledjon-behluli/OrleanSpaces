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

    public async Task RunAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            await agent.PopAsync(template, async tuple =>
            {
                string hash = (string)tuple[1];

#nullable disable
                if (hashPasswordPairs.TryGetValue(hash, out string password))
#nullable enable
                {
                    Console.WriteLine($"\nSLAVE {id}: Found password to Hash = {hash}");
                    await agent.WriteAsync(new((ExchangeKeys.PASSWORD_FOUND, hash, password)));
                }
            });

            await Task.Delay(1000, cancellationToken);  // Simulate some complex work from the slave.
        }
    }
}