using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Directors;
using OrleanSpaces.Tuples.Specialized;

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
        : base(IIntGrain.Key, IIntDirector.Key, options, client, router, observerChannel, callbackChannel) { }
}