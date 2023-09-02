using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Directors;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class DoubleProcessor : BaseProcessor<DoubleTuple, DoubleTemplate, IDoubleDirector>
{
    public DoubleProcessor(
        SpaceOptions options,
        IClusterClient client,
        ISpaceRouter<DoubleTuple, DoubleTemplate> router,
        ObserverChannel<DoubleTuple> observerChannel,
        CallbackChannel<DoubleTuple> callbackChannel)
        : base(IDoubleGrain.Key, IDoubleDirector.Key, options, client, router, observerChannel, callbackChannel) { }
}
