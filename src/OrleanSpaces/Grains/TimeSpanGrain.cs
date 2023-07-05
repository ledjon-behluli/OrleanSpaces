using Orleans.Runtime;
using OrleanSpaces.Tuples.Typed;

namespace OrleanSpaces.Grains;

internal interface ITimeSpanGrain : ITupleStore<TimeSpanTuple>, IGrainWithStringKey
{
    const string Key = "TimeSpanStore";
}

internal sealed class TimeSpanGrain : Grain<TimeSpanTuple>, ITimeSpanGrain
{
    public TimeSpanGrain(
        [PersistentState(ITimeSpanGrain.Key, Constants.StorageName)]
        IPersistentState<List<TimeSpanTuple>> space) : base(ITimeSpanGrain.Key, space) { }
}
