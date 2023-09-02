using OrleanSpaces.Channels;
using OrleanSpaces.Registries;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Agents;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class HugeAgent : BaseAgent<Int128, HugeTuple, HugeTemplate>
{
    public HugeAgent(
        SpaceOptions options,
        EvaluationChannel<HugeTuple> evaluationChannel,
        ObserverRegistry<HugeTuple> observerRegistry,
        CallbackRegistry<Int128, HugeTuple, HugeTemplate> callbackRegistry)
        : base(options, evaluationChannel, observerRegistry, callbackRegistry) { }
}