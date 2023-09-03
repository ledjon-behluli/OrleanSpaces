using OrleanSpaces.Channels;
using OrleanSpaces.Tuples.Specialized;
using OrleanSpaces.Grains.Directors;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class DecimalProcessor : BaseProcessor<DecimalTuple, DecimalTemplate, IDecimalDirector>
{
    public DecimalProcessor(
        SpaceClientOptions options,
        IClusterClient client,
        ISpaceRouter<DecimalTuple, DecimalTemplate> router,
        ObserverChannel<DecimalTuple> observerChannel,
        CallbackChannel<DecimalTuple> callbackChannel)
        : base(Constants.RealmKey_Decimal, options, client, router, observerChannel, callbackChannel) { }
}
