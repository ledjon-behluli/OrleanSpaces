using OrleanSpaces.Types;

namespace OrleanSpaces.Internals;

[Serializable]
internal struct TupleSpaceState
{
    public List<SpaceTuple> Tuples { get; set; }
}
