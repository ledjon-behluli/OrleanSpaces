using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class UHugeProcessor : BaseProcessor<UHugeTuple, UHugeTemplate>
{
    public UHugeProcessor(
        IClusterClient client,
        ISpaceRouter<UHugeTuple, UHugeTemplate> router,
        ObserverChannel<UHugeTuple> observerChannel,
        CallbackChannel<UHugeTuple> callbackChannel)
        : base(IUHugeGrain.Key, client, router, observerChannel, callbackChannel,
            () => client.GetGrain<IUHugeGrain>(IUHugeGrain.Key)) { }
}
