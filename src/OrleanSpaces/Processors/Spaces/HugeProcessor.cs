using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class HugeProcessor : BaseProcessor<HugeTuple, HugeTemplate>
{
    public HugeProcessor(
        SpaceOptions options,
        IClusterClient client,
        ISpaceRouter<HugeTuple, HugeTemplate> router,
        ObserverChannel<HugeTuple> observerChannel,
        CallbackChannel<HugeTuple> callbackChannel)
        : base(IHugeGrain.Key, options, client, router, observerChannel, callbackChannel, 
            () => client.GetGrain<IHugeGrain>(IHugeGrain.Key)) { }
}