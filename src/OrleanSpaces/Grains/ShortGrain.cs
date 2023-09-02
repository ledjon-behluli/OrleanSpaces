﻿using Orleans.Runtime;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains;

internal interface IShortGrain : ITupleStore<ShortTuple>, IGrainWithStringKey
{
    const string Key = "ShortStore";
}

internal sealed class ShortGrain : BaseGrain<ShortTuple>, IShortGrain
{
    public ShortGrain(
        [PersistentState(IShortGrain.Key, Constants.Store_StorageName)]
        IPersistentState<List<ShortTuple>> space) : base(IShortGrain.Key, space) { }
}
