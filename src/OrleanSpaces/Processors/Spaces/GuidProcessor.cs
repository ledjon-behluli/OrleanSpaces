using OrleanSpaces.Channels;
using OrleanSpaces.Tuples.Specialized;
using OrleanSpaces.Grains.Directors;

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
        : base(Constants.RealmKey_Guid, options, client, router, observerChannel, callbackChannel) { }
}
