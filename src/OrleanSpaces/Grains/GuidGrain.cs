﻿using Orleans.Runtime;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains;

internal interface IGuidGrain : ITupleStore<GuidTuple>, IGrainWithStringKey
{
    const string Key = "GuidStore";
}

internal sealed class GuidGrain : BaseGrain<GuidTuple>, IGuidGrain
{
    public GuidGrain(
        [PersistentState(IGuidGrain.Key, Constants.StorageName)]
        IPersistentState<List<GuidTuple>> space) : base(IGuidGrain.Key, space) { }
}
