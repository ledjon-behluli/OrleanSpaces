using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Directors;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class GuidProcessor : BaseProcessor<GuidTuple, GuidTemplate, IGuidDirector>
{
    public GuidProcessor(
        SpaceOptions options,
        IClusterClient client,
        ISpaceRouter<GuidTuple, GuidTemplate> router,
        ObserverChannel<GuidTuple> observerChannel,
        CallbackChannel<GuidTuple> callbackChannel)
        : base(IGuidGrain.Key, IGuidDirector.Key, options, client, router, observerChannel, callbackChannel) { }
}
