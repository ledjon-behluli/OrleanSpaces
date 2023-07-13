using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class DecimalProcessor : BaseProcessor<DecimalTuple, DecimalTemplate>
{
    public DecimalProcessor(
        IClusterClient client,
        ISpaceRouter<DecimalTuple, DecimalTemplate> router,
        ObserverChannel<DecimalTuple> observerChannel,
        CallbackChannel<DecimalTuple> callbackChannel)
        : base(IDecimalGrain.Key, client, router, observerChannel, callbackChannel, 
            () => client.GetGrain<IDecimalGrain>(IDecimalGrain.Key)) { }
}
