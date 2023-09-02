using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Interceptors;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class UIntProcessor : BaseProcessor<UIntTuple, UIntTemplate, IUIntInterceptor>
{
    public UIntProcessor(
        SpaceOptions options,
        IClusterClient client,
        ISpaceRouter<UIntTuple, UIntTemplate> router,
        ObserverChannel<UIntTuple> observerChannel,
        CallbackChannel<UIntTuple> callbackChannel)
        : base(IUIntGrain.Key, IUIntInterceptor.Key, options, client, router, observerChannel, callbackChannel) { }
}
