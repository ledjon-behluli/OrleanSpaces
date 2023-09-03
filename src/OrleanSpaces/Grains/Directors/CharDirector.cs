using Orleans.Runtime;
using OrleanSpaces.Grains.Stores;
using OrleanSpaces.Tuples.Specialized;

namespace OrleanSpaces.Grains.Directors;

internal interface ICharDirector : IStoreDirector<CharTuple>, IGrainWithStringKey { }

[ImplicitStreamSubscription(Constants.StreamName)]
internal sealed class CharDirector : BaseDirector<CharTuple, ICharStore>, ICharDirector
{
    public CharDirector(
        [PersistentState(Constants.RealmKey_Char, Constants.StorageName)]
        IPersistentState<DirectorState> state) : base(Constants.RealmKey_Char, state) {}
}