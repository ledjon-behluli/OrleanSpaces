using OrleanSpaces.Types;

namespace OrleanSpaces.Internals;

[Serializable]
internal struct SpaceState
{
    public List<SpaceTuple> Tuples { get; set; }
}
