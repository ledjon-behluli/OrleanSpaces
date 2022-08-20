using OrleanSpaces.Core.Primitives;

namespace OrleanSpaces.Hosts.Internals;

[Serializable]
internal struct SpaceState
{
    public List<SpaceTuple> Tuples { get; set; }
}
