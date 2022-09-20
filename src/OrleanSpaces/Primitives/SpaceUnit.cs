using Orleans.Concurrency;
using System.Runtime.CompilerServices;

namespace OrleanSpaces.Primitives;

/// <summary>
/// Represents an empty placeholder field and a unit type tuple, since <see langword="null"/> is not allowed as part of <see cref="SpaceTuple"/> and <see cref="SpaceTemplate"/>.
/// </summary>
[Immutable]
public readonly struct SpaceUnit : ITuple, IEquatable<SpaceUnit>, IComparable<SpaceUnit>
{
    private static readonly SpaceUnit @null = new();
    /// <summary>
    /// Default and only value of this type.
    /// </summary>
    /// <remarks><i>Should be used instead of the default constructor, to avoid unneccessary memory allocations.</i></remarks>
    public static ref readonly SpaceUnit Null => ref @null;

    /// <summary>
    /// Default constructor which always instantiates a <see cref="Null"/>. 
    /// </summary>
    /// <remarks><i>Use <see cref="Null"/> over this to avoid unneccessary memory allocations.</i></remarks>
    public SpaceUnit() { }

    int ITuple.Length => 1;
    object ITuple.this[int index] => index == 0 ? Null : throw new IndexOutOfRangeException();

    public bool Equals(SpaceUnit other) => true;
    public override bool Equals(object? obj) => obj is SpaceUnit;
    public override int GetHashCode() => 0;

    public int CompareTo(SpaceUnit other) => 0;

    public static bool operator ==(SpaceUnit left, SpaceUnit right) => true;
    public static bool operator !=(SpaceUnit left, SpaceUnit right) => false;

    public override string ToString() => "{NULL}";
}
