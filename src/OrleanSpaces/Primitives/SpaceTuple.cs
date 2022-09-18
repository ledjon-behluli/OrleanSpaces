using Orleans.Concurrency;
using System.Runtime.CompilerServices;

namespace OrleanSpaces.Primitives;

[Immutable]
public readonly struct SpaceTuple : ITuple, IEquatable<SpaceTuple>, IComparable<SpaceTuple>
{
    private readonly object[] fields;

    public object this[int index] => fields[index];
    public int Length => fields.Length;

    private static readonly SpaceTuple passive = new();
    public static ref readonly SpaceTuple Passive => ref passive;

    public bool IsPassive => Equals(Passive);

    public SpaceTuple() : this(new object[0])
    {
        
    }

    public SpaceTuple(params object[] fields)
    {
        if (fields == null)
        {
            throw new ArgumentNullException(nameof(fields));
        }

        if (fields.Length == 0)
        {
            this.fields = new object[1] { SpaceUnit.Null };
        }
        else
        {
            this.fields = new object[fields.Length];

            for (int i = 0; i < fields.Length; i++)
            {
                object obj = fields[i];

                if (!TypeChecker.IsSimpleType(obj.GetType()))
                {
                    throw new ArgumentException($"The field at position = {i}, is not a valid type. Allowed types are: strings, primitives and enums.");
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