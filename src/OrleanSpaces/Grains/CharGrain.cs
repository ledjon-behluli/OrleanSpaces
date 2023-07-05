﻿using Orleans.Runtime;
using OrleanSpaces.Tuples.Typed;

namespace OrleanSpaces.Grains;

internal interface ICharGrain : ITupleStore<CharTuple>, IGrainWithStringKey
{
    const string Key = "CharStore";
}

internal sealed class CharGrain : Grain<CharTuple>, ICharGrain
{
    public CharGrain(
        [PersistentState(ICharGrain.Key, Constants.StorageName)]
        IPersistentState<List<CharTuple>> space) : base(ICharGrain.Key, space) { }
}