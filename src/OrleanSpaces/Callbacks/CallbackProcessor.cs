using Microsoft.Extensions.Hosting;
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
                await continuationChannel.Writer.WriteAsync((SpaceTemplate)tuple, cancellationToken);
            }
        }
        catch
        {
            lifetime.StopApplication();
        }
    }
}