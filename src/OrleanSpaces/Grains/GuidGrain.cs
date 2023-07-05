﻿using Orleans.Runtime;
using OrleanSpaces.Tuples.Typed;

namespace OrleanSpaces.Grains;

internal interface IGuidGrain : ITupleStore<GuidTuple>, IGrainWithStringKey
{
    const string Key = "GuidStore";
}

internal sealed class GuidGrain : Grain<GuidTuple>, IGuidGrain
{
    public GuidGrain(
        [PersistentState(IGuidGrain.Key, Constants.StorageName)]
        IPersistentState<List<GuidTuple>> space) : base(IGuidGrain.Key, space) { }
}