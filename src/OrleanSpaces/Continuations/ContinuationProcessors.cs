using Microsoft.Extensions.Hosting;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Continuations;

internal sealed class TupleContinuationProcessor : BackgroundService
{
    private readonly ContinuationChannel channel;
    private readonly ITupleRouter router;

    public TupleContinuationProcessor(
        ContinuationChannel channel,
        ITupleRouter router)
    {
        this.channel = channel ?? throw new ArgumentNullException(nameof(channel));
        this.router = router ?? throw new ArgumentNullException(nameof(router));
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        channel.IsBeingConsumed = true;

        await foreach (SpaceTuple tuple in channel.TupleReader.ReadAllAsync(cancellationToken))
        {
            await router.RouteAsync(tuple);
        }

        channel.IsBeingConsumed = false;
    }
}

internal sealed class TemplateContinuationProcessor : BackgroundService
{
    private readonly ContinuationChannel channel;
    private readonly ITupleRouter router;

    public TemplateContinuationProcessor(
        ContinuationChannel channel,
        ITupleRouter router)
    {
        this.channel = channel ?? throw new ArgumentNullException(nameof(channel));
        this.router = router ?? throw new ArgumentNullException(nameof(router));
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        channel.IsBeingConsumed = true;

        await foreach (SpaceTemplate template in channel.TemplateReader.ReadAllAsync(cancellationToken))
        {
            await router.RouteAsync(template);
        }

        channel.IsBeingConsumed = false;
    }
}