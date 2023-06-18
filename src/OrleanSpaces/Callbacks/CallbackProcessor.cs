using Microsoft.Extensions.Hosting;
using OrleanSpaces.Continuations;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Callbacks;

internal sealed class CallbackProcessor<TTuple, TTemplate> : BackgroundService
    where TTuple : ISpaceTuple
    where TTemplate : ISpaceTemplate
{
    private readonly IHostApplicationLifetime lifetime;
    private readonly CallbackChannel<TTuple, TTemplate> callbackChannel;
    private readonly CallbackRegistry<TTuple, TTemplate> registry;
    private readonly ContinuationChannel<TTuple, TTemplate> continuationChannel;

    public CallbackProcessor(
        IHostApplicationLifetime lifetime,
        CallbackChannel<TTuple, TTemplate> callbackChannel,
        CallbackRegistry<TTuple, TTemplate> registry,
        ContinuationChannel<TTuple, TTemplate> continuationChannel)
    {
        this.lifetime = lifetime ?? throw new ArgumentNullException(nameof(lifetime));
        this.registry = registry ?? throw new ArgumentNullException(nameof(registry));
        this.callbackChannel = callbackChannel ?? throw new ArgumentNullException(nameof(callbackChannel));
        this.continuationChannel = continuationChannel ?? throw new ArgumentNullException(nameof(continuationChannel));
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        callbackChannel.IsBeingConsumed = true;

        await foreach (CallbackPair<TTuple, TTemplate> pair in callbackChannel.Reader.ReadAllAsync(cancellationToken))
        {
            List<Task> tasks = new();

            foreach (CallbackEntry<TTuple> entry in registry.Take(pair.Tuple))
            {
                tasks.Add(CallbackAsync(pair, entry, cancellationToken));
            }

            await Task.WhenAll(tasks);
        }

        callbackChannel.IsBeingConsumed = false;
    }

    private async Task CallbackAsync(CallbackPair<TTuple, TTemplate> pair, CallbackEntry<TTuple> entry, CancellationToken cancellationToken)
    {
        try
        {
            await entry.Callback(pair.Tuple);
            if (entry.IsContinuable)
            {
                await continuationChannel.TemplateWriter.WriteAsync(pair.Template, cancellationToken);
            }
        }
        catch
        {
            lifetime.StopApplication();
        }
    }
}