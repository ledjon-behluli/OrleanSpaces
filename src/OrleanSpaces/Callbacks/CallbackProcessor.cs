using Microsoft.Extensions.Hosting;
using OrleanSpaces.Continuations;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Callbacks;

internal sealed class CallbackProcessor<TTuple, TTemplate> : BackgroundService
    where TTuple : ISpaceTuple
    where TTemplate : ISpaceTemplate
{
    private readonly IHostApplicationLifetime lifetime;
    private readonly CallbackChannel<TTuple> callbackChannel;
    private readonly CallbackRegistry<TTuple, TTemplate> registry;
    private readonly ContinuationChannel<TTuple, TTemplate> continuationChannel;

    public CallbackProcessor(
        IHostApplicationLifetime lifetime,
        CallbackChannel<TTuple> callbackChannel,
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

        await foreach (TTuple tuple in callbackChannel.Reader.ReadAllAsync(cancellationToken))
        {
            List<Task> tasks = new();

            foreach (CallbackEntry<TTuple> entry in registry.Take(tuple))
            {
                tasks.Add(CallbackAsync(entry, tuple, cancellationToken));
            }

            await Task.WhenAll(tasks);
        }

        callbackChannel.IsBeingConsumed = false;
    }

    private async Task CallbackAsync(CallbackEntry<TTuple> entry, TTuple tuple, CancellationToken cancellationToken)
    {
        try
        {
            await entry.Callback(tuple);
            if (entry.IsContinuable)
            {
                await continuationChannel.TemplateWriter.WriteAsync(tuple.AsTemplate<TTemplate>(), cancellationToken);
            }
        }
        catch
        {
            lifetime.StopApplication();
        }
    }
}