using Microsoft.Extensions.Hosting;
using OrleanSpaces.Primitives;

namespace OrleanSpaces.Continuations;

internal class ContinuationProcessor : BackgroundService
{
    private readonly ContinuationChannel channel;
    private readonly ISpaceTupleRouter router;

    public ContinuationProcessor(
        ContinuationChannel channel,
        ISpaceTupleRouter router)
    {
        this.channel = channel ?? throw new ArgumentNullException(nameof(channel));
        this.router = router ?? throw new ArgumentNullException(nameof(router));
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await foreach (ISpaceTuple tuple in channel.Reader.ReadAllAsync(cancellationToken))
            await router.RouteAsync(tuple);
    }
}