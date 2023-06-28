using Microsoft.Extensions.Hosting;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Observers;

internal sealed class ObserverProcessor<T> : BackgroundService
    where T : ISpaceTuple
{
    private readonly ObserverChannel<T> channel;
    private readonly ObserverRegistry<T> registry;

    public ObserverProcessor(
        ObserverChannel<T> channel,
        ObserverRegistry<T> registry)
    {
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
                tasks.Add(observer.NotifyAsync(action, cancellationToken));
            }
            
            await Task.WhenAll(tasks);
        }
    }
}