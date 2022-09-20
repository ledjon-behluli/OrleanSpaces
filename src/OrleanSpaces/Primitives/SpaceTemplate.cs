using Orleans.Concurrency;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace OrleanSpaces.Primitives;

[Immutable]
public readonly partial struct SpaceTemplate : ITuple, IEquatable<SpaceTemplate>, IComparable<SpaceTemplate>
{
    private readonly object[] fields;

    public object this[int index] => fields[index];
    public int Length => fields.Length;

    /// <summary>
    /// Default constructor which always instantiates a <see cref="SpaceTemplate"/> with a single field of type <see cref="SpaceUnit"/>.
    /// </summary>
    public SpaceTemplate() : this(null)
    {
        
    }

    /// <summary>
    /// Main constructor which instantiates a <see cref="SpaceTemplate"/>, when all <paramref name="fields"/> are of valid type.
    /// </summary>
    /// <param name="fields">The fields of this template.</param>
    /// <remarks><i>Template fields can have types of: <see cref="SpaceUnit"/>, <see cref="Type"/>, <see cref="Type.IsPrimitive"/>, <see cref="Enum"/>, <see cref="string"/>, 
    /// <see cref="decimal"/>, <see cref="DateTime"/>, <see cref="DateTimeOffset"/>, <see cref="TimeSpan"/>, <see cref="Guid"/>.</i></remarks>
    /// <exception cref="ArgumentException"/>
    public SpaceTemplate([AllowNull] params object[] fields)
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

                Type type = obj.GetType();

                if (!TypeChecker.IsSimpleType(type) && type != typeof(SpaceUnit) && obj is not Type)
                {
                    throw new ArgumentException($"The field at position = {i} is not a valid type.");
                }

                this.fields[i] = obj;
            }
        }
    }

    public bool Matches(SpaceTuple tuple)
    {
        if (tuple.Length != Length)
        {
            return false;
        }

        for (int i = 0; i < tuple.Length; i++)
        {
            if (this[i] is not SpaceUnit)
            {
                if (this[i] is Type templateType)
                {
                    if (!templateType.Equals(tuple[i].GetType()))
                    {
                        return false;
                    }
                }
                else
                {
                    if (!this[i].Equals(tuple[i]))
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }

    public static implicit operator SpaceTemplate(SpaceTuple tuple)
    {
        object[] fields = new object[tuple.Length];

        for (int i = 0; i < tuple.Length; i++)
        {
            fields[i] = tuple[i];
        }

        return new(fields);
    }

    public static bool operator ==(SpaceTemplate left, SpaceTemplate right) => left.Equals(right);
    public static bool operator !=(SpaceTemplate left, SpaceTemplate right) => !(left == right);

    public override bool Equals(object obj) => obj is SpaceTemplate template && Equals(template);

    public bool Equals(SpaceTemplate other)
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

    public int CompareTo(SpaceTemplate other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();

    public override string ToString() => $"({string.Join(", ", fields)})";
}