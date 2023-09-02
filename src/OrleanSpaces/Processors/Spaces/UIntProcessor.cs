using OrleanSpaces.Channels;
using OrleanSpaces.Tuples.Specialized;
using OrleanSpaces.Grains.Stores;
using OrleanSpaces.Grains.Directors;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class UIntProcessor : BaseProcessor<UIntTuple, UIntTemplate, IUIntDirector>
{
    public UIntProcessor(
        SpaceOptions options,
        IClusterClient client,
        ISpaceRouter<UIntTuple, UIntTemplate> router,
        ObserverChannel<UIntTuple> observerChannel,
        CallbackChannel<UIntTuple> callbackChannel)
        : base(IUIntStore.Key, IUIntDirector.Key, options, client, router, observerChannel, callbackChannel) { }
}
