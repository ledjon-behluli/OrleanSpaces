using OrleanSpaces.Core;

namespace OrleanSpaces.Hosts.Internals;

[Serializable]
internal struct SpaceState
{
    public List<SpaceTuple> Tuples { get; set; }
}
