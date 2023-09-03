using OrleanSpaces.Channels;
using OrleanSpaces.Tuples.Specialized;
using OrleanSpaces.Grains.Directors;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class UShortProcessor : BaseProcessor<UShortTuple, UShortTemplate, IUShortDirector>
{
    public UShortProcessor(
        SpaceOptions options,
        IClusterClient client,
        ISpaceRouter<UShortTuple, UShortTemplate> router,
        ObserverChannel<UShortTuple> observerChannel,
        CallbackChannel<UShortTuple> callbackChannel)
        : base(Constants.RealmKey_UShort, options, client, router, observerChannel, callbackChannel) { }
}