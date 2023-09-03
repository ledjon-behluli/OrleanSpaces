using Orleans.Runtime;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains.Stores;

internal interface ICharStore : ITupleStore<CharTuple>, IGrainWithStringKey { }

internal sealed class CharStore : BaseStore<CharTuple>, ICharStore
{
    public CharStore(
        [PersistentState(Constants.RealmKey_Char, Constants.StorageName)]
        IPersistentState<List<CharTuple>> state) : base(Constants.RealmKey_Char, state) { }
}
