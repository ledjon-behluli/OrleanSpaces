﻿using OrleanSpaces.Channels;
using OrleanSpaces.Registries;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Agents;

[ImplicitStreamSubscription(Constants.Store_StreamNamespace)]
internal sealed class DoubleAgent : BaseAgent<double, DoubleTuple, DoubleTemplate>
{
    public DoubleAgent(
        SpaceOptions options,
        EvaluationChannel<DoubleTuple> evaluationChannel,
        ObserverRegistry<DoubleTuple> observerRegistry,
        CallbackRegistry<double, DoubleTuple, DoubleTemplate> callbackRegistry)
        : base(options, evaluationChannel, observerRegistry, callbackRegistry) { }
}