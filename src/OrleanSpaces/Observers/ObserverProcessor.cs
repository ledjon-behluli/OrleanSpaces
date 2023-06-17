using Microsoft.Extensions.Hosting;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Observers;

internal sealed class TupleObserverProcessor<TTuple, TTemplate> : BackgroundService
    where TTuple : ISpaceTuple
    where TTemplate : ISpaceTemplate
{
    private readonly IHostApplicationLifetime lifetime;
    private readonly ObserverRegistry<TTuple, TTemplate> registry;
    private readonly ObserverChannel<TTuple, TTemplate> channel;

    public TupleObserverProcessor(
        IHostApplicationLifetime lifetime,
        ObserverRegistry<TTuple, TTemplate> registry,
        ObserverChannel<TTuple, TTemplate> channel)
    {
        this.lifetime = lifetime ?? throw new ArgumentNullException(nameof(lifetime));
        this.registry = registry ?? throw new ArgumentNullException(nameof(registry));
        this.channel = channel ?? throw new ArgumentNullException(nameof(channel));
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        channel.IsBeingConsumed = true;

        // TODO: Look at 'ContinuationProcessor.cs' and perform same here to reader from TupleReader & TemplateReader
        await foreach (TTuple tuple in channel.TupleReader.ReadAllAsync(cancellationToken))
        {
            List<Task> tasks = new();

            foreach (SpaceObserver<TTuple, TTemplate> observer in registry.Observers)
            {
                tasks.Add(NotifyAsync(observer, tuple, cancellationToken));
            }
            
            await Task.WhenAll(tasks);
        }

        channel.IsBeingConsumed = false;
    }

    private async Task NotifyAsync(SpaceObserver<TTuple, TTemplate> observer, TTuple tuple, CancellationToken cancellationToken)
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