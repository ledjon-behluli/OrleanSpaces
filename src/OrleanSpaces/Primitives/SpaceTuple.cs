using System.Runtime.CompilerServices;

namespace OrleanSpaces.Primitives;

[Serializable]
public readonly struct SpaceTuple : ITuple, IEquatable<SpaceTuple>
{
    private readonly ITuple tuple;

    public object this[int index] => tuple[index];
    public int Length => tuple.Length;

    public bool IsUnit => Equals(Unit);//tuple[0] is SpaceUnit;

    private static readonly SpaceTuple unit = new();
    public static ref readonly SpaceTuple Unit => ref unit;

    public SpaceTuple()
    {
        tuple = SpaceUnit.Null;
    }

    public SpaceTuple(string value)
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }
     
        tuple = new ValueTuple<string>(value);
    }

    public SpaceTuple(ValueType valueType)
    {
        if (valueType is null)
        {
            throw new ArgumentNullException(nameof(valueType));
        }

        if (valueType is ITuple _tuple)
        {
            for (int i = 0; i < _tuple.Length; i++)
            {
                ThrowOnNotSupported(_tuple[i], i);
            }

            tuple = _tuple;
        }
        else
        {
            ThrowOnNotSupported(valueType);
            tuple = new ValueTuple<ValueType>(valueType);
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

    public override bool Equals(object obj) =>
        obj is SpaceTuple tuple && Equals(tuple);

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

    public override int GetHashCode() => tuple.GetHashCode();

    public override string ToString() => Length == 1 ? $"({tuple[0]})" : tuple.ToString();
}