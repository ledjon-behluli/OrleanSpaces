using Orleans.Concurrency;
using System.Runtime.CompilerServices;

namespace OrleanSpaces.Tuples;

/// <summary>
/// Represents an empty placeholder field and a unit tuple, since <see langword="null"/> is not allowed as part of <see cref="SpaceTuple"/> and <see cref="SpaceTemplate"/>.
/// </summary>
[Immutable]
public readonly struct SpaceUnit : ITuple, IEquatable<SpaceUnit>, IComparable<SpaceUnit>
{
    /// <summary>
    /// Default and only constructor. 
    /// </summary>
    public SpaceUnit() { }

    int ITuple.Length => 1;
    object ITuple.this[int index] => index == 0 ? this : throw new IndexOutOfRangeException();

    public static bool operator ==(SpaceUnit left, SpaceUnit right) => true;
    public static bool operator !=(SpaceUnit left, SpaceUnit right) => false;

    /// <summary>
    /// Determines whether the specified <see cref="object"/> is equal to this instance.
    /// </summary>
    /// <param name="obj">The object to compare with the current instance.</param>
    /// <returns><see langword="true"/>, if <paramref name="obj"/> is of type <see cref="SpaceUnit"/>; otherwise, <see langword="false"/>.</returns>
    public override bool Equals(object? obj) => obj is SpaceUnit;
    /// <summary>
    /// Determines whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>Always <see langword="true"/>, which means that this object is equal to any other instance of <see cref="SpaceUnit"/>.</returns>
    public bool Equals(SpaceUnit other) => true;

    /// <summary>
    /// Compares the current object with another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>Always 0, which means that this object is equal to any other instance of <see cref="SpaceUnit"/>.</returns>
    public int CompareTo(SpaceUnit other) => 0;

    public override int GetHashCode() => 0;
    public override string ToString() => "{NULL}";
}
