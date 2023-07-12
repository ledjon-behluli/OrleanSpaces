using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Registries;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Agents;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class DoubleAgent : BaseAgent<double, DoubleTuple, DoubleTemplate>
{
    public DoubleAgent(
        IClusterClient client,
        EvaluationChannel<DoubleTuple> evaluationChannel,
        ObserverRegistry<DoubleTuple> observerRegistry,
        CallbackRegistry<double, DoubleTuple, DoubleTemplate> callbackRegistry)
        : base(client.GetGrain<IDoubleGrain>(IDoubleGrain.Key), evaluationChannel, observerRegistry, callbackRegistry) { }
}