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

    internal SpaceTuple(object[] fields)
    {
        this.fields = fields;
    }

    public SpaceTuple()
    {
        fields = new object[1] { SpaceUnit.Null };
    }

    public SpaceTuple(string value)
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        fields = new object[1] { value };
    }

    public SpaceTuple(ValueType valueType)
    {
        if (valueType is null)
        {
            throw new ArgumentNullException(nameof(valueType));
        }

        if (valueType is ITuple _tuple)
        {
            fields = new object[_tuple.Length];
            for (int i = 0; i < _tuple.Length; i++)
            {
                ThrowOnNotSupported(_tuple[i], i);
                fields[i] = _tuple[i];
            }
        }
        else
        {
            ThrowOnNotSupported(valueType);
            fields = new object[1] { valueType };
        }

        static void ThrowOnNotSupported(object obj, int index = 0)
        {
            if (!TypeChecker.IsSimpleType(obj.GetType()))
            {
                throw new ArgumentException($"The field at position = {index}, is not a valid type. Allowed types include: strings, primitives and enums.");
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