using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Interceptors;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Processors.Spaces;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class CharProcessor : BaseProcessor<CharTuple, CharTemplate, ICharInterceptor>
{
    public CharProcessor(
        SpaceOptions options,
        IClusterClient client,
        ISpaceRouter<CharTuple, CharTemplate> router,
        ObserverChannel<CharTuple> observerChannel,
        CallbackChannel<CharTuple> callbackChannel)
        : base(ICharGrain.Key, ICharInterceptor.Key, options, client, router, observerChannel, callbackChannel)
    { }
}
