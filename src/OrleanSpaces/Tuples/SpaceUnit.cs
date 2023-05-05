using Orleans.Concurrency;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples;

/// <summary>
/// Represents an empty placeholder field and a unit tuple, since <see langword="null"/> is not allowed as part of <see cref="SpaceTuple"/> and <see cref="SpaceTemplate"/>.
/// </summary>
[Immutable]
public readonly struct SpaceUnit : IObjectTuple<SpaceUnit, SpaceUnit>, ISpanFormattable
{
    internal const string DefaultString = "{NULL}";
    internal static readonly SpaceUnit Default = new();

    int ISpaceTuple.Length => 1;
    SpaceUnit IObjectTuple<SpaceUnit, SpaceUnit>.this[int index] => index == 0 ? this : throw new IndexOutOfRangeException();

    /// <summary>
    /// Default and only constructor. 
    /// </summary>
    public SpaceUnit() { }
    
    [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Two SpaceUnit types are always equal to each other.")]
    public static bool operator ==(SpaceUnit left, SpaceUnit right) => true;

    [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Two SpaceUnit types can never be not equal to each other.")] 
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
    public override string ToString() => DefaultString;

    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "For consistency with other implementations.")]
    public bool TryFormat(Span<char> destination, out int charsWritten)
    {
        ReadOnlySpan<char> span = DefaultString.AsSpan();

        if (destination.Length < span.Length)
        {
            charsWritten = 0;
            return false;
        }

        span.CopyTo(destination);
        charsWritten = span.Length;

        return true;
    }

    bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
        => TryFormat(destination, out charsWritten);

    string IFormattable.ToString(string? format, IFormatProvider? formatProvider) => ToString();
}
