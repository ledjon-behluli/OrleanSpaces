using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Registries;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Agents;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class UIntAgent : BaseAgent<uint, UIntTuple, UIntTemplate>
{
    public UIntAgent(
        IClusterClient client,
        EvaluationChannel<UIntTuple> evaluationChannel,
        ObserverRegistry<UIntTuple> observerRegistry,
        CallbackRegistry<uint, UIntTuple, UIntTemplate> callbackRegistry)
        : base(client.GetGrain<IUIntGrain>(IUIntGrain.Key), evaluationChannel, observerRegistry, callbackRegistry) { }
}