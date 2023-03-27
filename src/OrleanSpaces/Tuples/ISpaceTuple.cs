using System.Numerics;

namespace OrleanSpaces.Tuples;

public interface ISpaceTuple<T, H> : IEquatable<H>, IComparable<H>
    where T : struct
    where H : ISpaceTuple<T, H>
{
    T this[int index] { get; }
    int Length { get; }
}

public interface ITypedSpaceTuple<T, H> : ISpaceTuple<T, H>
    where T : struct
    where H : ISpaceTuple<T, H>
{
    Vector<T> Fields { get; }
}

class T
{
    public T()
    {
        IntegerTuple tuple = new IntegerTuple(new[] {1, 2, 3, 4});

    }
}

public readonly struct IntegerTuple : ITypedSpaceTuple<int, IntegerTuple>
{
    public readonly Vector<int> Fields { get; }

    public int this[int index] => Fields[index];
    public int Length { get; }

    public IntegerTuple(int[] fields)
    {
        Length = fields.Length;
        Fields = new Vector<int>(fields);
    }

    public IntegerTuple(Span<int> fields)
    {
        Length = fields.Length;
        Fields = new Vector<int>(fields);
    }

    public static bool operator ==(IntegerTuple left, IntegerTuple right) => left.Equals(right);
    public static bool operator !=(IntegerTuple left, IntegerTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is IntegerTuple tuple && Equals(tuple);

    public bool Equals(IntegerTuple other)
    {
        if (Length != other.Length)
        {
            return false;
        }

        if (Vector.IsHardwareAccelerated)
        {
            return Vector.EqualsAll(Fields, other.Fields);
        }

        return FallbackEquals(other);
    }

    private bool FallbackEquals(IntegerTuple other)
    {
        for (int i = 0; i < Length; i++)
        {
            if (!this[i].Equals(other[i]))
            {
                return false;
            }
        }

        return true;
    }

    public int CompareTo(IntegerTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => Fields.GetHashCode();

    public override string ToString() => $"({string.Join(", ", Fields)})";
}