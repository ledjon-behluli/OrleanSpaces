using OrleanSpaces.Channels;
using OrleanSpaces.Tuples.Specialized;
using OrleanSpaces.Grains.Directors;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class IntProcessor : BaseProcessor<IntTuple, IntTemplate, IIntDirector>
{
    public IntProcessor(
        SpaceClientOptions options,
        IClusterClient client,
        ISpaceRouter<IntTuple, IntTemplate> router,
        ObserverChannel<IntTuple> observerChannel,
        CallbackChannel<IntTuple> callbackChannel)
        : base(Constants.RealmKey_Int, options, client, router, observerChannel, callbackChannel) { }
}