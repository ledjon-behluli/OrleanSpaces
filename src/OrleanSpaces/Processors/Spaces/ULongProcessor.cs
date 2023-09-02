using OrleanSpaces.Channels;
using OrleanSpaces.Tuples.Specialized;
using OrleanSpaces.Grains.Stores;
using OrleanSpaces.Grains.Directors;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class ULongProcessor : BaseProcessor<ULongTuple, ULongTemplate, IULongDirector>
{
    public ULongProcessor(
        SpaceOptions options,
        IClusterClient client,
        ISpaceRouter<ULongTuple, ULongTemplate> router,
        ObserverChannel<ULongTuple> observerChannel,
        CallbackChannel<ULongTuple> callbackChannel)
        : base(IULongStore.Key, IULongDirector.Key, options, client, router, observerChannel, callbackChannel) { }
}
