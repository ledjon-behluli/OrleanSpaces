using OrleanSpaces.Core.Primitives;

namespace OrleanSpaces.Hosts.Grains;

[Serializable]
internal struct SpaceState
{
    public List<SpaceTuple> Tuples { get; set; }
}
