using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Registries;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Agents;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class UHugeAgent : BaseAgent<UInt128, UHugeTuple, UHugeTemplate>
{
    public UHugeAgent(
        IClusterClient client,
        EvaluationChannel<UHugeTuple> evaluationChannel,
        ObserverRegistry<UHugeTuple> observerRegistry,
        CallbackRegistry<UInt128, UHugeTuple, UHugeTemplate> callbackRegistry)
        : base(client.GetGrain<IUHugeGrain>(IUHugeGrain.Key), evaluationChannel, observerRegistry, callbackRegistry) { }
}