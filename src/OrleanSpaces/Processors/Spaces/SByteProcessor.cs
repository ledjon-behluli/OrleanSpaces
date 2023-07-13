using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class SByteProcessor : BaseProcessor<SByteTuple, SByteTemplate>
{
    public SByteProcessor(
        IClusterClient client,
        ISpaceRouter<SByteTuple, SByteTemplate> router,
        ObserverChannel<SByteTuple> observerChannel,
        CallbackChannel<SByteTuple> callbackChannel)
        : base(ISByteGrain.Key, client, router, observerChannel, callbackChannel,
            () => client.GetGrain<ISByteGrain>(ISByteGrain.Key)) { }
}
