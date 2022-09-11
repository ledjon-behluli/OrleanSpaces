using System.Runtime.CompilerServices;

namespace OrleanSpaces.Primitives;

[Serializable]
public struct SpaceTuple : ITuple, IEquatable<SpaceTuple>
{
    private readonly ITuple? tuple;

    public object this[int index] => (tuple ?? throw new IndexOutOfRangeException())[index];

    public int Length => tuple?.Length ?? 0;

    public bool IsEmpty => Length == 0;

    public SpaceTuple() : this(null) { }
    private SpaceTuple(ITuple? tuple) => this.tuple = tuple;

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
    }

    private static void ThrowOnNotSupported(object obj, int index = 0)
    {
        if (!TypeChecker.IsSimpleType(obj.GetType()))
        {
            throw new ArgumentException(
                $"The field at position = {index}, is not a valid '{nameof(SpaceTuple)}' member. " +
                $"Valid members are: '{nameof(String)}', '{nameof(ValueType)}'");
        }
    }

    public static SpaceTuple Create(string value)
    {
        if (string.IsNullOrEmpty(value))
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

    public override int GetHashCode() => HashCode.Combine(tuple, Length);

    public override string ToString() => tuple?.ToString() ?? "()";
}