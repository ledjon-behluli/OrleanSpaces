using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class DoubleProcessor : BaseProcessor<DoubleTuple, DoubleTemplate>
{
    public DoubleProcessor(
        IClusterClient client,
        ISpaceRouter<DoubleTuple, DoubleTemplate> router,
        ObserverChannel<DoubleTuple> observerChannel,
        CallbackChannel<DoubleTuple> callbackChannel)
        : base(IDoubleGrain.Key, client, router, observerChannel, callbackChannel,
            () => client.GetGrain<IDoubleGrain>(IDoubleGrain.Key)) { }
}
