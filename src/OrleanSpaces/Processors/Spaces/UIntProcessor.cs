using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class UIntProcessor : BaseProcessor<UIntTuple, UIntTemplate>
{
    public UIntProcessor(
        IClusterClient client,
        ISpaceRouter<UIntTuple, UIntTemplate> router,
        ObserverChannel<UIntTuple> observerChannel,
        CallbackChannel<UIntTuple> callbackChannel)
        : base(IUIntGrain.Key, client, router, observerChannel, callbackChannel, 
            () => client.GetGrain<IUIntGrain>(IUIntGrain.Key)) { }
}
