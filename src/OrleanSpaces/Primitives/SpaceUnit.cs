namespace OrleanSpaces.Primitives;

[Serializable]
public readonly struct SpaceUnit : IEquatable<SpaceUnit>
{
    private static readonly SpaceUnit @null = new();
    public static ref readonly SpaceUnit Null => ref @null;

    public bool Equals(SpaceUnit other) => true;
    public override bool Equals(object? obj) => obj is SpaceUnit;
    public override int GetHashCode() => 0;

    public static bool operator ==(SpaceUnit first, SpaceUnit second) => true;
    public static bool operator !=(SpaceUnit first, SpaceUnit second) => false;

    public override string ToString() => "{NULL}";
}
