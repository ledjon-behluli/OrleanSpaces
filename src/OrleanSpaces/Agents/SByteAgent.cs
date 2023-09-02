﻿using OrleanSpaces.Channels;
using OrleanSpaces.Registries;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Agents;

[ImplicitStreamSubscription(Constants.Store_StreamNamespace)]
internal sealed class SByteAgent : BaseAgent<sbyte, SByteTuple, SByteTemplate>
{
    public SByteAgent(
        SpaceOptions options,
        EvaluationChannel<SByteTuple> evaluationChannel,
        ObserverRegistry<SByteTuple> observerRegistry,
        CallbackRegistry<sbyte, SByteTuple, SByteTemplate> callbackRegistry)
        : base(options, evaluationChannel, observerRegistry, callbackRegistry) { }
}