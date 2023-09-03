﻿using Orleans.Runtime;
using OrleanSpaces.Grains.Stores;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains.Directors;

internal interface IDateTimeDirector : IStoreDirector<DateTimeTuple>, IGrainWithStringKey { }

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class DateTimeDirector : BaseDirector<DateTimeTuple, IDateTimeStore>, IDateTimeDirector
{
    public DateTimeDirector(
        [PersistentState(Constants.RealmKey_DateTime, Constants.StorageName)]
        IPersistentState<HashSet<string>> storeKeys)
        : base(Constants.RealmKey_DateTime, storeKeys) { }
}