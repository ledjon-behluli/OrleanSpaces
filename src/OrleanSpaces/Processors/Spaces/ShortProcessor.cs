using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class ShortProcessor : BaseProcessor<ShortTuple, ShortTemplate>
{
    public ShortProcessor(
        IClusterClient client,
        ISpaceRouter<ShortTuple, ShortTemplate> router,
        ObserverChannel<ShortTuple> observerChannel,
        CallbackChannel<ShortTuple> callbackChannel)
        : base(IShortGrain.Key, client, router, observerChannel, callbackChannel,
            () => client.GetGrain<IShortGrain>(IShortGrain.Key)) { }
}
