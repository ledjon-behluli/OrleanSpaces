using OrleanSpaces.Channels;
using OrleanSpaces.Tuples.Specialized;
using OrleanSpaces.Grains.Directors;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class BoolProcessor : BaseProcessor<BoolTuple, BoolTemplate, IBoolDirector>
{
    public BoolProcessor(
        SpaceOptions options,
        IClusterClient client,
        ISpaceRouter<BoolTuple, BoolTemplate> router,
        ObserverChannel<BoolTuple> observerChannel,
        CallbackChannel<BoolTuple> callbackChannel)
        : base(Constants.RealmKey_Bool, options, client, router, observerChannel, callbackChannel) { }
}