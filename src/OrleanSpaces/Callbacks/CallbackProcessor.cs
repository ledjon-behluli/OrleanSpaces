using Microsoft.Extensions.Hosting;
using OrleanSpaces.Continuations;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Callbacks;

internal sealed class CallbackProcessor : BackgroundService
{
    private readonly CallbackRegistry registry;
    private readonly CallbackChannel<SpaceTuple> callbackChannel;
    private readonly ContinuationChannel<SpaceTuple, SpaceTemplate> continuationChannel;

    public CallbackProcessor(
        CallbackRegistry registry,
        CallbackChannel<SpaceTuple> callbackChannel,
        ContinuationChannel<SpaceTuple, SpaceTemplate> continuationChannel)
    {
        this.registry = registry ?? throw new ArgumentNullException(nameof(registry));
        this.callbackChannel = callbackChannel ?? throw new ArgumentNullException(nameof(callbackChannel));
        this.continuationChannel = continuationChannel ?? throw new ArgumentNullException(nameof(continuationChannel));
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await foreach (SpaceTuple tuple in callbackChannel.Reader.ReadAllAsync(cancellationToken))
        {
            List<Task> tasks = new();

            foreach (CallbackEntry<SpaceTuple> entry in registry.Take(tuple))
            {
                tasks.Add(CallbackAsync(tuple, entry, cancellationToken));
            }

            await Task.WhenAll(tasks);
        }
    }

    private async Task CallbackAsync(SpaceTuple tuple, CallbackEntry<SpaceTuple> entry, CancellationToken cancellationToken)
    {
        await entry.Callback(tuple);
        if (entry.HasContinuation)
        {
            await continuationChannel.TemplateWriter.WriteAsync((SpaceTemplate)tuple, cancellationToken);
        }
    }
}

internal sealed class CallbackProcessor<T, TTuple, TTemplate> : BackgroundService
    where T : unmanaged
    where TTuple : ISpaceTuple<T>
    where TTemplate : ISpaceTemplate<T>
{
    private readonly CallbackChannel<TTuple> callbackChannel;
    private readonly CallbackRegistry<T, TTuple, TTemplate> registry;
    private readonly ContinuationChannel<TTuple, TTemplate> continuationChannel;

    public CallbackProcessor(
        CallbackRegistry<T, TTuple, TTemplate> registry,
        CallbackChannel<TTuple> callbackChannel,
        ContinuationChannel<TTuple, TTemplate> continuationChannel)
    {
        this.registry = registry ?? throw new ArgumentNullException(nameof(registry));
        this.callbackChannel = callbackChannel ?? throw new ArgumentNullException(nameof(callbackChannel));
        this.continuationChannel = continuationChannel ?? throw new ArgumentNullException(nameof(continuationChannel));
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await foreach (TTuple tuple in callbackChannel.Reader.ReadAllAsync(cancellationToken))
        {
            List<Task> tasks = new();

            foreach (CallbackEntry<TTuple> entry in registry.Take(tuple))
            {
                tasks.Add(CallbackAsync(tuple, entry, cancellationToken));
            }

            await Task.WhenAll(tasks);
        }
    }

    private async Task CallbackAsync(TTuple tuple, CallbackEntry<TTuple> entry, CancellationToken cancellationToken)
    {
        await entry.Callback(tuple);
        if (entry.HasContinuation)
        {
            await continuationChannel.TemplateWriter.WriteAsync((TTemplate)tuple.ToTemplate(), cancellationToken);
        }
    }
}