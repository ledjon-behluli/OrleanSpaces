using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class SByteProcessor : BaseProcessor<SByteTuple, SByteTemplate>
{
    public SByteProcessor(
        SpaceOptions options,
        IClusterClient client,
        ISpaceRouter<SByteTuple, SByteTemplate> router,
        ObserverChannel<SByteTuple> observerChannel,
        CallbackChannel<SByteTuple> callbackChannel)
        : base(ISByteGrain.Key, options, client, router, observerChannel, callbackChannel,
            () => client.GetGrain<ISByteGrain>(ISByteGrain.Key)) { }
}
