using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class UShortProcessor : BaseProcessor<UShortTuple, UShortTemplate>
{
    public UShortProcessor(
        IClusterClient client,
        ISpaceRouter<UShortTuple, UShortTemplate> router,
        ObserverChannel<UShortTuple> observerChannel,
        CallbackChannel<UShortTuple> callbackChannel)
        : base(IUShortGrain.Key, client, router, observerChannel, callbackChannel,
            () => client.GetGrain<IUShortGrain>(IUShortGrain.Key)) { }
}