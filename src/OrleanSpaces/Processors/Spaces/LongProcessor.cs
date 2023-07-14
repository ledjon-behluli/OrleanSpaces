using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class LongProcessor : BaseProcessor<LongTuple, LongTemplate>
{
    public LongProcessor(
        SpaceOptions options,
        IClusterClient client,
        ISpaceRouter<LongTuple, LongTemplate> router,
        ObserverChannel<LongTuple> observerChannel,
        CallbackChannel<LongTuple> callbackChannel)
        : base(ILongGrain.Key, options, client, router, observerChannel, callbackChannel, 
            () => client.GetGrain<ILongGrain>(ILongGrain.Key)) { }
}
