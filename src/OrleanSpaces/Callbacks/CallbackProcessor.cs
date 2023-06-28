using Microsoft.Extensions.Hosting;
using OrleanSpaces.Continuations;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Callbacks;

internal sealed class CallbackProcessor : BackgroundService
{
    private readonly CallbackRegistry registry;
    private readonly CallbackChannel<SpaceTuple, SpaceTemplate> callbackChannel;
    private readonly ContinuationChannel<SpaceTuple, SpaceTemplate> continuationChannel;

    public CallbackProcessor(
        CallbackRegistry registry,
        CallbackChannel<SpaceTuple, SpaceTemplate> callbackChannel,
        ContinuationChannel<SpaceTuple, SpaceTemplate> continuationChannel)
    {
        this.registry = registry ?? throw new ArgumentNullException(nameof(registry));
        this.callbackChannel = callbackChannel ?? throw new ArgumentNullException(nameof(callbackChannel));
        this.continuationChannel = continuationChannel ?? throw new ArgumentNullException(nameof(continuationChannel));
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await foreach (CallbackPair<SpaceTuple, SpaceTemplate> pair in callbackChannel.Reader.ReadAllAsync(cancellationToken))
        {
            List<Task> tasks = new();

            foreach (CallbackEntry<SpaceTuple> entry in registry.Take(pair.Tuple))
            {
                tasks.Add(CallbackAsync(pair, entry, cancellationToken));
            }

            await Task.WhenAll(tasks);
        }
    }

    private async Task CallbackAsync(CallbackPair<SpaceTuple, SpaceTemplate> pair, CallbackEntry<SpaceTuple> entry, CancellationToken cancellationToken)
    {
        await entry.Callback(pair.Tuple);
        if (entry.IsContinuable)
        {
            await continuationChannel.TemplateWriter.WriteAsync(pair.Template, cancellationToken);
        }
    }
}

internal sealed class CallbackProcessor<T, TTuple, TTemplate> : BackgroundService
    where T : unmanaged
    where TTuple : ISpaceTuple<T>
    where TTemplate : ISpaceTemplate<T>
{
    private readonly CallbackChannel<TTuple, TTemplate> callbackChannel;
    private readonly CallbackRegistry<T, TTuple, TTemplate> registry;
    private readonly ContinuationChannel<TTuple, TTemplate> continuationChannel;

    public CallbackProcessor(
        CallbackChannel<TTuple, TTemplate> callbackChannel,
        CallbackRegistry<T, TTuple, TTemplate> registry,
        ContinuationChannel<TTuple, TTemplate> continuationChannel)
    {
        this.registry = registry ?? throw new ArgumentNullException(nameof(registry));
        this.callbackChannel = callbackChannel ?? throw new ArgumentNullException(nameof(callbackChannel));
        this.continuationChannel = continuationChannel ?? throw new ArgumentNullException(nameof(continuationChannel));
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await foreach (CallbackPair<TTuple, TTemplate> pair in callbackChannel.Reader.ReadAllAsync(cancellationToken))
        {
            List<Task> tasks = new();

            foreach (CallbackEntry<TTuple> entry in registry.Take(pair.Tuple))
            {
                tasks.Add(CallbackAsync(pair, entry, cancellationToken));
            }

            await Task.WhenAll(tasks);
        }
    }

    private async Task CallbackAsync(CallbackPair<TTuple, TTemplate> pair, CallbackEntry<TTuple> entry, CancellationToken cancellationToken)
    {
        await entry.Callback(pair.Tuple);
        if (entry.IsContinuable)
        {
            await continuationChannel.TemplateWriter.WriteAsync(pair.Template, cancellationToken);
        }
    }
}