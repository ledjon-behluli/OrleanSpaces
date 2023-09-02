using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Directors;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class BoolProcessor : BaseProcessor<BoolTuple, BoolTemplate, IBoolDirector>
{
    public BoolProcessor(
        SpaceOptions options,
        IClusterClient client,
        ISpaceRouter<BoolTuple, BoolTemplate> router,
        ObserverChannel<BoolTuple> observerChannel,
        CallbackChannel<BoolTuple> callbackChannel)
        : base(IBoolGrain.Key, IBoolDirector.Key, options, client, router, observerChannel, callbackChannel) { }
}