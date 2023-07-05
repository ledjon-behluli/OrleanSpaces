﻿using Orleans.Runtime;
using OrleanSpaces.Tuples.Typed;

namespace OrleanSpaces.Grains;

internal interface IUShortGrain : ITupleStore<UShortTuple>, IGrainWithStringKey
{
    const string Key = "UShortStore";
}


internal sealed class UShortGrain : Grain<UShortTuple>, IUShortGrain
{
    public UShortGrain(
        [PersistentState(IUShortGrain.Key, Constants.StorageName)]
        IPersistentState<List<UShortTuple>> space) : base(IUShortGrain.Key, space) { }
}