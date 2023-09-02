using OrleanSpaces.Channels;
using OrleanSpaces.Tuples.Specialized;
using OrleanSpaces.Grains.Stores;
using OrleanSpaces.Grains.Directors;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class LongProcessor : BaseProcessor<LongTuple, LongTemplate, ILongDirector>
{
    public LongProcessor(
        SpaceOptions options,
        IClusterClient client,
        ISpaceRouter<LongTuple, LongTemplate> router,
        ObserverChannel<LongTuple> observerChannel,
        CallbackChannel<LongTuple> callbackChannel)
        : base(ILongStore.Key, ILongDirector.Key, options, client, router, observerChannel, callbackChannel) { }
}
