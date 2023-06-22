using Microsoft.Extensions.Hosting;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Observers;

internal sealed class ObserverProcessor<T> : BackgroundService
    where T : ISpaceTuple
{
    private readonly IHostApplicationLifetime lifetime;
    private readonly ObserverChannel<T> channel;
    private readonly ObserverRegistry<T> registry;

    public ObserverProcessor(
        IHostApplicationLifetime lifetime,
        ObserverChannel<T> channel,
        ObserverRegistry<T> registry)
    {
        this.lifetime = lifetime ?? throw new ArgumentNullException(nameof(lifetime));
        this.registry = registry ?? throw new ArgumentNullException(nameof(registry));
        this.channel = channel ?? throw new ArgumentNullException(nameof(channel));
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await foreach (TupleAction<T> action in channel.Reader.ReadAllAsync(cancellationToken))
        {
            List<Task> tasks = new();

            foreach (SpaceObserver<T> observer in registry.Observers)
            {
                tasks.Add(NotifyAsync(observer, action, cancellationToken));
            }
            
            await Task.WhenAll(tasks);
        }
    }

    private async Task NotifyAsync(SpaceObserver<T> observer, TupleAction<T> action, CancellationToken cancellationToken)
    {
        try
        {
            await observer.NotifyAsync(action, cancellationToken);
        }
        catch
        {
            lifetime.StopApplication();
        }
    }
}