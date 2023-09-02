using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Directors;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class DecimalProcessor : BaseProcessor<DecimalTuple, DecimalTemplate, IDecimalDirector>
{
    public DecimalProcessor(
        SpaceOptions options,
        IClusterClient client,
        ISpaceRouter<DecimalTuple, DecimalTemplate> router,
        ObserverChannel<DecimalTuple> observerChannel,
        CallbackChannel<DecimalTuple> callbackChannel)
        : base(IDecimalGrain.Key, IDecimalDirector.Key, options, client, router, observerChannel, callbackChannel) { }
}
