using Orleans.Concurrency;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace OrleanSpaces.Primitives;

/// <summary>
/// Represents a tuple in the tuple space paradigm.
/// </summary>
[Immutable]
public readonly struct SpaceTuple : ITuple, IEquatable<SpaceTuple>, IComparable<SpaceTuple>
{
    private readonly object[] fields;

    public object this[int index] => fields[index];
    public int Length => fields.Length;

    private static readonly SpaceTuple passive = new();
    /// <summary>
    /// Represents a <see cref="SpaceTuple"/> with a single field of type <see cref="SpaceUnit"/>.
    /// </summary>
    /// <remarks><i>Use over the default constructor to avoid unneccessary memory allocations.</i></remarks>
    public static ref readonly SpaceTuple Passive => ref passive;

    /// <summary>
    /// Checks wether this instance is a <see cref="Passive"/>.
    /// </summary>
    public bool IsPassive => Equals(Passive);

    /// <summary>
    /// Default constructor which always instantiates a <see cref="Passive"/>. 
    /// </summary>
    /// <remarks><i>Use <see cref="Passive"/> over this to avoid unneccessary memory allocations.</i></remarks>
    public SpaceTuple() : this(null) { }

    /// <summary>
    /// Main constructor which instantiates a non-<see cref="Passive"/>, when all <paramref name="fields"/> are of valid type.
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

    public override bool Equals(object obj) => obj is SpaceTuple tuple && Equals(tuple);

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

    public int CompareTo(SpaceTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();

    public override string ToString() => $"({string.Join(", ", fields)})";
}