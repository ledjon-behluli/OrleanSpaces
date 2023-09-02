using OrleanSpaces.Channels;
using OrleanSpaces.Tuples.Specialized;
using OrleanSpaces.Grains.Stores;
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
        : base(IUShortStore.Key, IUShortDirector.Key, options, client, router, observerChannel, callbackChannel) { }
}