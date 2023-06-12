using Microsoft.Extensions.Hosting;
using OrleanSpaces.Tuples;

namespace OrleanSpaces.Continuations;

internal sealed class ContinuationProcessor : BackgroundService
{
    private readonly ContinuationChannel channel;
    private readonly ITupleRouter router;

    public ContinuationProcessor(
        ContinuationChannel channel,
        ITupleRouter router)
    {
        this.channel = channel ?? throw new ArgumentNullException(nameof(channel));
        this.router = router ?? throw new ArgumentNullException(nameof(router));
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        channel.IsBeingConsumed = true;

        await foreach (ISpaceElement tuple in channel.Reader.ReadAllAsync(cancellationToken))
        {
            await router.RouteAsync(tuple);
        }

        channel.IsBeingConsumed = false;
    }
}