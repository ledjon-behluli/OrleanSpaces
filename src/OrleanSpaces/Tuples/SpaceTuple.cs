using Orleans.Concurrency;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace OrleanSpaces.Tuples;

/// <summary>
/// Represents a tuple in the tuple space paradigm.
/// </summary>
[Immutable]
public readonly struct SpaceTuple : ITuple, IEquatable<SpaceTuple>, IComparable<SpaceTuple>
{
    private readonly object[] fields;

    public object this[int index] => fields[index];
    public int Length => fields.Length;

    private static readonly SpaceTuple @null = new();
    /// <summary>
    /// Represents a <see cref="SpaceTuple"/> with a single field of type <see cref="SpaceUnit"/>.
    /// </summary>
    /// <remarks><i>Use over the default constructor to avoid unneccessary memory allocations.</i></remarks>
    public static ref readonly SpaceTuple Null => ref @null;

    /// <summary>
    /// Checks wether this instance is a <see cref="Null"/>.
    /// </summary>
    public bool IsNull => Equals(Null);

    /// <summary>
    /// Default constructor which always instantiates a <see cref="Null"/>. 
    /// </summary>
    /// <remarks><i>Use <see cref="Null"/> over this to avoid unneccessary memory allocations.</i></remarks>
    public SpaceTuple() : this(null) { }

    /// <summary>
    /// Main constructor which instantiates a non-<see cref="Null"/>, when all <paramref name="fields"/> are of valid type.
    /// </summary>
    /// <param name="fields">The fields of this tuple.</param>
    /// <remarks><i>Tuple fields can have types of: <see cref="Type.IsPrimitive"/>, <see cref="Enum"/>, <see cref="string"/>, 
    /// <see cref="decimal"/>, <see cref="DateTime"/>, <see cref="DateTimeOffset"/>, <see cref="TimeSpan"/>, <see cref="Guid"/>.</i></remarks>
    /// <exception cref="ArgumentException"/>
    public SpaceTuple([AllowNull] params object[] fields)
    {
        if (fields == null || fields.Length == 0)
        {
            this.fields = new object[1] { SpaceUnit.Null };
        }
        else
        {
            this.fields = new object[fields.Length];

            for (int i = 0; i < fields.Length; i++)
            {
                object obj = fields[i];
                if (obj is null)
                {
                    throw new ArgumentException($"The field at position = {i} can not be null.");
                }

                if (!TypeChecker.IsSimpleType(obj.GetType()))
                {
                    throw new ArgumentException($"The field at position = {i} is not a valid type.");
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
    /// <returns><see langword="true"/>, if <see langword="this"/> and <paramref name="other"/> share the same number of fields, and all of them match on the type, value and index; otherwise, <see langword="false"/>.</returns>
    public bool Equals(SpaceTuple other)
    {
        if (Length != other.Length)
        {
            return false;
        }

        for (int i = 0; i < Length; i++)
        {
            if (!this[i].Equals(other[i]))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Compares the current object with another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>Whatever the result of length comparison between <see langword="this"/> and <paramref name="other"/> is.</returns>
    public int CompareTo(SpaceTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();

    public override string ToString() => $"({string.Join(", ", fields)})";
}