using OrleanSpaces.Channels;
using OrleanSpaces.Tuples.Specialized;
using OrleanSpaces.Grains.Stores;
using OrleanSpaces.Grains.Directors;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class IntProcessor : BaseProcessor<IntTuple, IntTemplate, IIntDirector>
{
    public IntProcessor(
        SpaceOptions options,
        IClusterClient client,
        ISpaceRouter<IntTuple, IntTemplate> router,
        ObserverChannel<IntTuple> observerChannel,
        CallbackChannel<IntTuple> callbackChannel)
        : base(IIntStore.Key, IIntDirector.Key, options, client, router, observerChannel, callbackChannel) { }
}