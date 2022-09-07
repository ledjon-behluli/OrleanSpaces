using Microsoft.Extensions.Hosting;
using OrleanSpaces.Primitives;
using OrleanSpaces.Utils;

namespace OrleanSpaces.Observers;

internal class ObserverProcessor : BackgroundService
{
    private readonly ObserverRegistry registry;
    private readonly ObserverChannel channel;

    public ObserverProcessor(
        ObserverRegistry registry,
        ObserverChannel channel)
    {
        this.registry = registry ?? throw new ArgumentNullException(nameof(registry));
        this.channel = channel ?? throw new ArgumentNullException(nameof(channel));
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await foreach (SpaceTuple tuple in channel.Reader.ReadAllAsync(cancellationToken))
        {
            await TaskPartitioner.WhenAll(registry.Observers, async x =>
            {
                try
                {
                    await x.OnTupleAsync(tuple);
                }
                catch (Exception)
                {

                }
            });
        }
    }
}
