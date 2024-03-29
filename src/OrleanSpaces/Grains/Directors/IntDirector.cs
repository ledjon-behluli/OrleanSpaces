﻿using Orleans.Runtime;
using OrleanSpaces.Grains.Stores;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains.Directors;

internal interface IIntDirector : IStoreDirector<IntTuple>, IGrainWithStringKey { }

internal sealed class IntDirector : BaseDirector<IntTuple, IIntStore>, IIntDirector
{
    public IntDirector(
        [PersistentState(Constants.RealmKey_Int, Constants.StorageName)]
        IPersistentState<DirectorState> state) : base(Constants.RealmKey_Int, state) {}
}
