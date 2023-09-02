using OrleanSpaces.Channels;
using OrleanSpaces.Registries;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Agents;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class UHugeAgent : BaseAgent<UInt128, UHugeTuple, UHugeTemplate>
{
    public UHugeAgent(
        SpaceOptions options,
        EvaluationChannel<UHugeTuple> evaluationChannel,
        ObserverRegistry<UHugeTuple> observerRegistry,
        CallbackRegistry<UInt128, UHugeTuple, UHugeTemplate> callbackRegistry)
        : base(options, evaluationChannel, observerRegistry, callbackRegistry) { }
}