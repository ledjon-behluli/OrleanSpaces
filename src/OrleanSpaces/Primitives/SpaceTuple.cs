using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace OrleanSpaces.Primitives;

[Serializable]
public readonly struct SpaceTuple : ITuple, IEquatable<SpaceTuple>
{
    private readonly ITuple tuple;

    public object this[int index] => tuple[index];
    public int Length => tuple.Length;

    public bool IsEmpty => tuple[0] is SpaceUnit;

    private SpaceTuple(ITuple tuple) => this.tuple = tuple;

    public SpaceTuple() : this(SpaceUnit.Null) { }
    
    public static SpaceTuple Create(ValueType value)
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        if (value is ITuple tuple)
        {
            for (int i = 0; i < tuple.Length; i++)
            {
                ThrowOnNotSupported(tuple[i], i);
            }

            return new(tuple);
        }

        ThrowOnNotSupported(value);
        
        return new(new ValueTuple<ValueType>(value));

        static void ThrowOnNotSupported(object obj, int index = 0)
        {
            if (!TypeChecker.IsSimpleType(obj.GetType()))
            {
                throw new ArgumentException($"The field at position = {index}, is not a valid type. Allowed types include: strings, primitives and enums.");
            }
        }
    }

    public static SpaceTuple Create([NotNull] string value)
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        return new(new ValueTuple<string>(value));
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

    public override string ToString() => IsEmpty ? "()" : tuple.ToString();
}