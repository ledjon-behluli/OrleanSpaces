using Microsoft.Extensions.Hosting;
using OrleanSpaces.Channels;
using OrleanSpaces.Registries;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Processors;

internal sealed class CallbackProcessor : BackgroundService
{
    private readonly SpaceClientOptions options;
    private readonly CallbackRegistry registry;
    private readonly CallbackChannel<SpaceTuple> callbackChannel;
    private readonly ContinuationChannel<SpaceTuple, SpaceTemplate> continuationChannel;

    public CallbackProcessor(
        SpaceClientOptions options,
        CallbackRegistry registry,
        CallbackChannel<SpaceTuple> callbackChannel,
        ContinuationChannel<SpaceTuple, SpaceTemplate> continuationChannel)
    {
        this.options = options;
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
        try
        {
            await entry.Callback(tuple);
        }
        catch
        {
            if (!options.HandleCallbackExceptions)
            {
                throw;
            }
        }

        if (entry.HasContinuation)
        {
            await continuationChannel.TemplateWriter.WriteAsync(tuple.ToTemplate(), cancellationToken);
        }
    }
}

internal sealed class CallbackProcessor<T, TTuple, TTemplate> : BackgroundService
    where T : unmanaged
    where TTuple : ISpaceTuple<T>, ISpaceConvertible<T, TTemplate>
    where TTemplate : ISpaceTemplate<T>, ISpaceMatchable<T, TTuple>
{
    private readonly SpaceClientOptions options;
    private readonly CallbackChannel<TTuple> callbackChannel;
    private readonly CallbackRegistry<T, TTuple, TTemplate> registry;
    private readonly ContinuationChannel<TTuple, TTemplate> continuationChannel;

    public CallbackProcessor(
        SpaceClientOptions options,
        CallbackRegistry<T, TTuple, TTemplate> registry,
        CallbackChannel<TTuple> callbackChannel,
        ContinuationChannel<TTuple, TTemplate> continuationChannel)
    {
        this.options = options;
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
        try
        {
            await entry.Callback(tuple);
        }
        catch
        {
            if (!options.HandleCallbackExceptions)
            {
                throw;
            }
        }

        if (entry.HasContinuation)
        {
            await continuationChannel.TemplateWriter.WriteAsync(tuple.ToTemplate(), cancellationToken);
        }
    }
}