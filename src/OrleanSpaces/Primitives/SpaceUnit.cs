using Orleans.Concurrency;
using System.Runtime.CompilerServices;

namespace OrleanSpaces.Primitives;

[Immutable]
[Serializable]
public readonly struct SpaceUnit : ITuple, IEquatable<SpaceUnit>, IComparable<SpaceUnit>
{
    private static readonly SpaceUnit @null = new();
    public static ref readonly SpaceUnit Null => ref @null;

    int ITuple.Length => 1;
    object ITuple.this[int index] => Null;

    public bool Equals(SpaceUnit other) => true;
    public override bool Equals(object? obj) => obj is SpaceUnit;
    public override int GetHashCode() => 0;

    public int CompareTo(SpaceUnit other) => 0;

    public static bool operator ==(SpaceUnit left, SpaceUnit right) => true;
    public static bool operator !=(SpaceUnit left, SpaceUnit right) => false;

    public override string ToString() => "{NULL}";
}
