using Microsoft.Extensions.Hosting;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Observers;

internal sealed class TupleObserverProcessor : BackgroundService
{
    private readonly IHostApplicationLifetime lifetime;
    private readonly ObserverRegistry registry;
    private readonly ObserverChannel channel;

    public TupleObserverProcessor(
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

        await foreach (SpaceTuple tuple in channel.TupleReader.ReadAllAsync(cancellationToken))
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

    private async Task NotifyAsync(SpaceObserver observer, SpaceTuple tuple, CancellationToken cancellationToken)
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

internal sealed class TemplateObserverProcessor : BackgroundService
{
    private readonly IHostApplicationLifetime lifetime;
    private readonly ObserverRegistry registry;
    private readonly ObserverChannel channel;

    public TemplateObserverProcessor(
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

        await foreach (SpaceTemplate tuple in channel.TemplateReader.ReadAllAsync(cancellationToken))
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

    private async Task NotifyAsync(SpaceObserver observer, SpaceTemplate template, CancellationToken cancellationToken)
    {
        try
        {
            await observer.NotifyAsync(template, cancellationToken);
        }
        catch
        {
            lifetime.StopApplication();
        }
    }
}