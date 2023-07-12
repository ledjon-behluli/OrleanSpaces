using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Registries;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Agents;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class UShortAgent : BaseAgent<ushort, UShortTuple, UShortTemplate>
{
    public UShortAgent(
        IClusterClient client,
        EvaluationChannel<UShortTuple> evaluationChannel,
        ObserverRegistry<UShortTuple> observerRegistry,
        CallbackRegistry<ushort, UShortTuple, UShortTemplate> callbackRegistry)
        : base(client.GetGrain<IUShortGrain>(IUShortGrain.Key), evaluationChannel, observerRegistry, callbackRegistry) { }
}