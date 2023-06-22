using OrleanSpaces;
using Microsoft.Extensions.Hosting;
using OrleanSpaces.Tuples;

public class Worker : BackgroundService
{
    private readonly ISpaceAgentProvider provider;
    private readonly IHostApplicationLifetime lifetime;

    public Worker(
        ISpaceAgentProvider provider,
        IHostApplicationLifetime lifetime)
    {
        this.provider = provider;
        this.lifetime = lifetime;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        ISpaceAgent agent = await provider.GetAsync();

        SpaceTuple tuple = new(1, 2, 3);

        await agent.WriteAsync(tuple);

        SpaceTemplate template = new(1, null, 3);

        Console.WriteLine($"Searching for matching tuple with template: {template}");

        var _tuple = await agent.PeekAsync(template);
        if (_tuple.Length > 0)
        {
            Console.WriteLine($"Found this tuple: {_tuple}");
        }

        _tuple = await agent.PopAsync(template);
        if (_tuple.Length > 0)
        {
            Console.WriteLine($"Found this tuple: {_tuple} and removed it");
        }

        _tuple = await agent.PeekAsync(template);
        if (_tuple.Length == 0)
        {
            Console.WriteLine($"Tuple: {_tuple} has been removed");
        }

        Console.WriteLine("\nPress any key to terminate...");
        Console.ReadKey();

        lifetime.StopApplication();
    }
}