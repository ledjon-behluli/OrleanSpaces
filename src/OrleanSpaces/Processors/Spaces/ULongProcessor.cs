using OrleanSpaces.Channels;
using OrleanSpaces.Tuples.Specialized;
using OrleanSpaces.Grains.Directors;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class ULongProcessor : BaseProcessor<ULongTuple, ULongTemplate, IULongDirector>
{
    public ULongProcessor(
        SpaceClientOptions options,
        IClusterClient client,
        ISpaceRouter<ULongTuple, ULongTemplate> router,
        ObserverChannel<ULongTuple> observerChannel,
        CallbackChannel<ULongTuple> callbackChannel)
        : base(Constants.RealmKey_ULong, options, client, router, observerChannel, callbackChannel) { }
}
