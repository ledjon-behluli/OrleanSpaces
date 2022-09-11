using Microsoft.Extensions.Hosting;
using System.Runtime.CompilerServices;

namespace OrleanSpaces.Continuations;

internal class ContinuationProcessor : BackgroundService
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
        await foreach (ITuple tuple in channel.Reader.ReadAllAsync(cancellationToken))
            await router.RouteAsync(tuple);
    }
}