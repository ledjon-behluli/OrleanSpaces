using OrleanSpaces.Channels;
using OrleanSpaces.Tuples.Specialized;
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
        : base(Constants.RealmKey_Long, options, client, router, observerChannel, callbackChannel) { }
}
