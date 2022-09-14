using Microsoft.Extensions.Hosting;
using System.Runtime.CompilerServices;

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
        await foreach (ITuple tuple in channel.Reader.ReadAllAsync(cancellationToken))
        {
            List<Task> tasks = new();

            foreach (var observer in registry.Observers)
            {
                tasks.Add(NotifyAsync(observer, tuple, cancellationToken));
            }
            
            await Task.WhenAll(tasks);
        }
    }

    private async Task NotifyAsync(DynamicObserver observer, ITuple tuple, CancellationToken cancellationToken)
    {
        try
        {
            await observer.HandleAsync(tuple, cancellationToken);
        }
        catch
        {
            lifetime.StopApplication();
        }
    }
}
