using Microsoft.Extensions.Hosting;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Observers;

internal sealed class ObserverProcessor : BackgroundService
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
        channel.IsBeingConsumed = true;

        await foreach (ISpaceTuple tuple in channel.Reader.ReadAllAsync(cancellationToken))
        {
            List<Task> tasks = new();

            foreach (SpaceObserver observer in registry.Observers)
            {
                tasks.Add(NotifyAsync(observer, tuple, cancellationToken));
            }
            
            await Task.WhenAll(tasks);
        }

        channel.IsBeingConsumed = false;
    }

    private async Task NotifyAsync(SpaceObserver observer, ISpaceTuple tuple, CancellationToken cancellationToken)
    {
        try
        {
            await observer.NotifyAsync(tuple, cancellationToken);
        }
        catch
        {
            lifetime.StopApplication();
        }
    }
}
