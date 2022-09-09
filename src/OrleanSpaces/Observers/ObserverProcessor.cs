using Microsoft.Extensions.Hosting;
using OrleanSpaces.Primitives;

namespace OrleanSpaces.Observers;

internal class ObserverProcessor : BackgroundService
{
    private readonly IHostApplicationLifetime lifetime;
    private readonly ObserverRegistry registry;
    private readonly ObserverChannel channel;

    public ObserverProcessor(
        IHostApplicationLifetime lifetime,
        ObserverRegistry registry,
        ObserverChannel channel)
    {
        this.lifetime = lifetime ?? throw new ArgumentNullException(nameof(lifetime));
        this.registry = registry ?? throw new ArgumentNullException(nameof(registry));
        this.channel = channel ?? throw new ArgumentNullException(nameof(channel));
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await foreach (SpaceTuple tuple in channel.Reader.ReadAllAsync(cancellationToken))
        {
            List<Task> tasks = new();

            foreach (var observer in registry.Observers)
            {
                tasks.Add(NotifyAsync(observer, tuple, cancellationToken));
            }
            
            await Task.WhenAll(tasks);
        }
    }

    private async Task NotifyAsync(ISpaceObserver observer, SpaceTuple tuple, CancellationToken cancellationToken)
    {
        try
        {
            if (!tuple.IsEmpty)
            {
                await observer.OnTupleAsync(tuple, cancellationToken);
            }
            else
            {
                await observer.OnEmptySpaceAsync(cancellationToken);
            }
        }
        catch
        {
            lifetime.StopApplication();
        }
    }
}
