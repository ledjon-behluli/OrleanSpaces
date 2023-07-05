﻿using Orleans.Runtime;
using OrleanSpaces.Tuples.Typed;

namespace OrleanSpaces.Grains;

internal interface IShortGrain : ITupleStore<ShortTuple>, IGrainWithStringKey
{
    const string Key = "ShortStore";
}

internal sealed class ShortGrain : Grain<ShortTuple>, IShortGrain
{
    public ShortGrain(
        [PersistentState(IShortGrain.Key, Constants.StorageName)]
        IPersistentState<List<ShortTuple>> space) : base(IShortGrain.Key, space) { }
}