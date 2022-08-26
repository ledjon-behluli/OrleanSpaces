using OrleanSpaces.Primitives;

namespace OrleanSpaces.Grains;

[Serializable]
internal class SpaceState
{
    public List<SpaceTuple> Tuples { get; set; } = new();
}
