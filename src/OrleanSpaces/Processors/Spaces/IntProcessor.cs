using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class IntProcessor : BaseProcessor<IntTuple, IntTemplate>
{
    public IntProcessor(
        IClusterClient client,
        ISpaceRouter<IntTuple, IntTemplate> router,
        ObserverChannel<IntTuple> observerChannel,
        CallbackChannel<IntTuple> callbackChannel)
        : base(IIntGrain.Key, client, router, observerChannel, callbackChannel,
            () => client.GetGrain<IIntGrain>(IIntGrain.Key)) { }
}