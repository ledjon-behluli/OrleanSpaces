using OrleanSpaces.Core.Primitives;

namespace OrleanSpaces.Hosts;

[Serializable]
internal struct SpaceState
{
    public List<SpaceTuple> Tuples { get; set; }
}
