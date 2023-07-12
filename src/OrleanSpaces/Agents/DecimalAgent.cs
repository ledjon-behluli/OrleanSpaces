using OrleanSpaces.Channels;
using OrleanSpaces.Grains;
using OrleanSpaces.Registries;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Agents;

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class DecimalAgent : BaseAgent<decimal, DecimalTuple, DecimalTemplate>
{
    public DecimalAgent(
        IClusterClient client,
        EvaluationChannel<DecimalTuple> evaluationChannel,
        ObserverRegistry<DecimalTuple> observerRegistry,
        CallbackRegistry<decimal, DecimalTuple, DecimalTemplate> callbackRegistry)
        : base(client.GetGrain<IDecimalGrain>(IDecimalGrain.Key), evaluationChannel, observerRegistry, callbackRegistry) { }
}