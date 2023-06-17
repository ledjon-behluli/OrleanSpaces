using Microsoft.Extensions.Hosting;
using Microsoft.Win32;
using OrleanSpaces.Continuations;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Callbacks;

internal sealed class CallbackProcessor : BackgroundService
{
    private readonly IHostApplicationLifetime lifetime;
    private readonly CallbackRegistry registry;
    private readonly CallbackChannel callbackChannel;
    private readonly ContinuationChannel continuationChannel;
        
    public CallbackProcessor(
        IHostApplicationLifetime lifetime,
        CallbackRegistry registry,
        CallbackChannel callbackChannel,
        ContinuationChannel continuationChannel)
    {
        this.lifetime = lifetime ?? throw new ArgumentNullException(nameof(lifetime));
        this.registry = registry ?? throw new ArgumentNullException(nameof(registry));
        this.callbackChannel = callbackChannel ?? throw new ArgumentNullException(nameof(callbackChannel));
        this.continuationChannel = continuationChannel ?? throw new ArgumentNullException(nameof(continuationChannel));
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        callbackChannel.IsBeingConsumed = true;

        await foreach (SpaceTuple tuple in callbackChannel.Reader.ReadAllAsync(cancellationToken))
        {
            List<Task> tasks = new();

            foreach (CallbackEntry entry in registry.Take(tuple))
            {
                tasks.Add(CallbackAsync(entry, tuple, cancellationToken));
            }

            await Task.WhenAll(tasks);
        }

        callbackChannel.IsBeingConsumed = false;
    }

    private async Task CallbackAsync(CallbackEntry entry, SpaceTuple tuple, CancellationToken cancellationToken)
    {
        try
        {
            await entry.Callback(tuple);
            if (entry.IsContinuable)
            {
                await continuationChannel.TemplateWriter.WriteAsync((SpaceTemplate)tuple, cancellationToken);
            }
        }
        catch
        {
            lifetime.StopApplication();
        }
    }
}

internal sealed class CallbackProcessor<T, TTuple, TTemplate> : BackgroundService
    where T : unmanaged
    where TTuple : ISpaceTuple<T>
    where TTemplate : ISpaceTemplate<T>
{
    private readonly IHostApplicationLifetime lifetime;
    private readonly CallbackChannel<T, TTuple> callbackChannel;
    private readonly CallbackRegistry<T, TTuple, TTemplate> registry;
    private readonly ContinuationChannel<T, TTuple, TTemplate> continuationChannel;

    public CallbackProcessor(
        IHostApplicationLifetime lifetime,
        CallbackChannel<T, TTuple> callbackChannel,
        CallbackRegistry<T, TTuple, TTemplate> registry,
        ContinuationChannel<T, TTuple, TTemplate> continuationChannel)
    {
        this.lifetime = lifetime ?? throw new ArgumentNullException(nameof(lifetime));
        this.registry = registry ?? throw new ArgumentNullException(nameof(registry));
        this.callbackChannel = callbackChannel ?? throw new ArgumentNullException(nameof(callbackChannel));
        this.continuationChannel = continuationChannel ?? throw new ArgumentNullException(nameof(continuationChannel));
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        callbackChannel.IsBeingConsumed = true;

        await foreach (TTuple tuple in callbackChannel.Reader.ReadAllAsync(cancellationToken))
        {
            List<Task> tasks = new();

            foreach (CallbackEntry<T, TTuple> entry in registry.Take(tuple))
            {
                tasks.Add(CallbackAsync(entry, tuple, cancellationToken));
            }

            await Task.WhenAll(tasks);
        }

        callbackChannel.IsBeingConsumed = false;
    }

    private async Task CallbackAsync(CallbackEntry<T, TTuple> entry, TTuple tuple, CancellationToken cancellationToken)
    {
        try
        {
            await entry.Callback(tuple);
            if (entry.IsContinuable)
            {
                await continuationChannel.TemplateWriter.WriteAsync<TTemplate>(tuple.ToTemplate(), cancellationToken);
            }
        }
        catch
        {
            lifetime.StopApplication();
        }
    }
}