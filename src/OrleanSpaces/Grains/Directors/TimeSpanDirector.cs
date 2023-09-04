using Orleans.Runtime;
using OrleanSpaces.Grains.Stores;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains.Directors;

internal interface ITimeSpanDirector : IStoreDirector<TimeSpanTuple>, IGrainWithStringKey { }

internal sealed class TimeSpanDirector : BaseDirector<TimeSpanTuple, ITimeSpanStore>, ITimeSpanDirector
{
    public TimeSpanDirector(
        [PersistentState(Constants.RealmKey_TimeSpan, Constants.StorageName)]
        IPersistentState<DirectorState> state) : base(Constants.RealmKey_TimeSpan, state) {}
}