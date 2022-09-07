using Microsoft.Extensions.Hosting;
using OrleanSpaces.Continuations;
using OrleanSpaces.Primitives;
using OrleanSpaces.Utils;

namespace OrleanSpaces.Callbacks;

internal class CallbackProcessor : BackgroundService
{
    private readonly CallbackRegistry registry;
    private readonly CallbackChannel callbackChannel;
    private readonly ContinuationChannel continuationChannel;
    
    public CallbackProcessor(
        CallbackRegistry registry,
        CallbackChannel callbackChannel,
        ContinuationChannel continuationChannel)
    {
        this.registry = registry ?? throw new ArgumentNullException(nameof(registry));
        this.callbackChannel = callbackChannel ?? throw new ArgumentNullException(nameof(callbackChannel));
        this.continuationChannel = continuationChannel ?? throw new ArgumentNullException(nameof(continuationChannel));
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await foreach (var tuple in callbackChannel.Reader.ReadAllAsync(cancellationToken))
        {
            var entries = registry.Take(tuple);
            await TaskPartitioner.WhenAll(entries, async entry =>
            {
                try
                {
                    await entry.Callback(tuple);
                    if (entry.IsDestructive)
                    {
                        await continuationChannel.Writer.WriteAsync(SpaceTemplate.Create(tuple));
                    }
                }
                catch (Exception)
                {
                    
                }
            });
        }
    }
}