using OrleanSpaces.Channels;
using OrleanSpaces.Tuples.Specialized;
using OrleanSpaces.Grains.Stores;
using OrleanSpaces.Grains.Directors;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class SByteProcessor : BaseProcessor<SByteTuple, SByteTemplate, ISByteDirector>
{
    public SByteProcessor(
        SpaceOptions options,
        IClusterClient client,
        ISpaceRouter<SByteTuple, SByteTemplate> router,
        ObserverChannel<SByteTuple> observerChannel,
        CallbackChannel<SByteTuple> callbackChannel)
        : base(ISByteStore.Key, ISByteDirector.Key, options, client, router, observerChannel, callbackChannel) { }
}
