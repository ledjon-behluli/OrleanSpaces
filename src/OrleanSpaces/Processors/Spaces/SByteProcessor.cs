using OrleanSpaces.Channels;
using OrleanSpaces.Tuples.Specialized;
using OrleanSpaces.Grains.Directors;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class SByteProcessor : BaseProcessor<SByteTuple, SByteTemplate, ISByteDirector>
{
    public SByteProcessor(
        SpaceClientOptions options,
        IClusterClient client,
        ISpaceRouter<SByteTuple, SByteTemplate> router,
        ObserverChannel<SByteTuple> observerChannel,
        CallbackChannel<SByteTuple> callbackChannel)
        : base(Constants.RealmKey_SByte, options, client, router, observerChannel, callbackChannel) { }
}
