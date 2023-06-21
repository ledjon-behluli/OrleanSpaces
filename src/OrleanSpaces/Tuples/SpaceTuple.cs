using Orleans.Concurrency;

namespace OrleanSpaces.Tuples;

/// <summary>
/// Represents a tuple in the tuple space paradigm.
/// </summary>
[Immutable]
public readonly struct SpaceTuple : ISpaceTuple, IEquatable<SpaceTuple>
{
    private readonly object[] fields;

    internal static SpaceTuple Empty => new();

    public readonly object this[int index] => fields[index];
    public int Length => fields.Length;

    /// <summary>
    /// Default constructor which always instantiates a <see cref="Null"/>. 
    /// </summary>
    public SpaceTuple() : this(Array.Empty<object>()) { }

    /// <summary>
    /// Main constructor which instantiates a non-<see cref="Null"/>, when all <paramref name="fields"/> are of valid type.
    /// </summary>
    /// <param name="fields">The ticks of this tuple.</param>
    /// <remarks><i>Tuple ticks can have types of: <see cref="Type.IsPrimitive"/>, <see cref="Enum"/>, <see cref="string"/>, 
    /// <see cref="decimal"/>, <see cref="DateTime"/>, <see cref="DateTimeOffset"/>, <see cref="TimeSpan"/>, <see cref="Guid"/>.</i></remarks>
    /// <exception cref="ArgumentException"/>
    public SpaceTuple(params object[] fields)
    {
        if (fields is null)
        {
            this.fields = Array.Empty<object>();
            return;
        }

        this.fields = new object[fields.Length];

        for (int i = 0; i < fields.Length; i++)
        {
            object? obj = fields[i];
            if (obj is null)
            {
                ThrowHelpers.NullField(i);
            }
            else
            {
                if (!obj.GetType().IsSupportedType())
                {
                    ThrowHelpers.InvalidFieldType(i);
                }

                this.fields[i] = obj;
            }
        }
    }

    public static bool operator ==(SpaceTuple left, SpaceTuple right) => left.Equals(right);
    public static bool operator !=(SpaceTuple left, SpaceTuple right) => !(left == right);

    /// <summary>
    /// Determines whether the specified <see cref="object"/> is equal to this instance.
    /// </summary>
    /// <param name="obj">The object to compare with the current instance.</param>
    /// <returns><see langword="true"/>, if <paramref name="obj"/> is of type <see cref="SpaceTuple"/> and <see cref="Equals(SpaceTuple)"/> returns <see langword="true"/>; otherwise, <see langword="false"/>.</returns>
    public override bool Equals(object? obj) => obj is SpaceTuple tuple && Equals(tuple);
    /// <summary>
    /// Determines whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns><see langword="true"/>, if <see langword="this"/> and <paramref name="other"/> share the same number of ticks, and all of them match on the type, value and index; otherwise, <see langword="false"/>.</returns>
    public bool Equals(SpaceTuple other)
    {
        if (Length != other.Length)
        {
            return false;
        }

        for (int i = 0; i < Length; i++)
        {
            if (!this.Equals(other))
            {
                return false;
            }
        }

        return true;
    }

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => $"({string.Join(", ", fields)})";

    public ReadOnlySpan<char> AsSpan() => ReadOnlySpan<char>.Empty;
    public ReadOnlySpan<object>.Enumerator GetEnumerator() => new ReadOnlySpan<object>(fields).GetEnumerator();
}