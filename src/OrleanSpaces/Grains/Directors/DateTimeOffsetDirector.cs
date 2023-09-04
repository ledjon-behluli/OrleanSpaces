using Orleans.Runtime;
using OrleanSpaces.Grains.Stores;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains.Directors;

internal interface IDateTimeOffsetDirector : IStoreDirector<DateTimeOffsetTuple>, IGrainWithStringKey { }

internal sealed class DateTimeOffsetDirector : BaseDirector<DateTimeOffsetTuple, IDateTimeOffsetStore>, IDateTimeOffsetDirector
{
    public DateTimeOffsetDirector(
        [PersistentState(Constants.RealmKey_DateTimeOffset, Constants.StorageName)]
        IPersistentState<DirectorState> state) : base(Constants.RealmKey_DateTimeOffset, state) {}
}