using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Directors;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class ShortProcessor : BaseProcessor<ShortTuple, ShortTemplate, IShortDirector>
{
    public ShortProcessor(
        SpaceOptions options,
        IClusterClient client,
        ISpaceRouter<ShortTuple, ShortTemplate> router,
        ObserverChannel<ShortTuple> observerChannel,
        CallbackChannel<ShortTuple> callbackChannel)
        : base(IShortGrain.Key, IShortDirector.Key, options, client, router, observerChannel, callbackChannel) { }
}
