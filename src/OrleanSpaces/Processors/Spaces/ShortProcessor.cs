using OrleanSpaces.Channels;
using OrleanSpaces.Tuples.Specialized;
using OrleanSpaces.Grains.Directors;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class ShortProcessor : BaseProcessor<ShortTuple, ShortTemplate, IShortDirector>
{
    public ShortProcessor(
        SpaceClientOptions options,
        IClusterClient client,
        ISpaceRouter<ShortTuple, ShortTemplate> router,
        ObserverChannel<ShortTuple> observerChannel,
        CallbackChannel<ShortTuple> callbackChannel)
        : base(Constants.RealmKey_Short, options, client, router, observerChannel, callbackChannel) { }
}
