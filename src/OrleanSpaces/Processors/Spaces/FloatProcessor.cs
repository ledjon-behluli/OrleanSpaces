using OrleanSpaces.Channels;
using OrleanSpaces.Tuples.Specialized;
using OrleanSpaces.Grains.Directors;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class FloatProcessor : BaseProcessor<FloatTuple, FloatTemplate, IFloatDirector>
{
    public FloatProcessor(
        SpaceClientOptions options,
        IClusterClient client,
        ISpaceRouter<FloatTuple, FloatTemplate> router,
        ObserverChannel<FloatTuple> observerChannel,
        CallbackChannel<FloatTuple> callbackChannel)
        : base(Constants.RealmKey_Float, options, client, router, observerChannel, callbackChannel) { }
}
