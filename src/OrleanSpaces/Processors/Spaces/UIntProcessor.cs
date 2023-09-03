using OrleanSpaces.Channels;
using OrleanSpaces.Tuples.Specialized;
using OrleanSpaces.Grains.Directors;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class UIntProcessor : BaseProcessor<UIntTuple, UIntTemplate, IUIntDirector>
{
    public UIntProcessor(
        SpaceClientOptions options,
        IClusterClient client,
        ISpaceRouter<UIntTuple, UIntTemplate> router,
        ObserverChannel<UIntTuple> observerChannel,
        CallbackChannel<UIntTuple> callbackChannel)
        : base(Constants.RealmKey_UInt, options, client, router, observerChannel, callbackChannel) { }
}
