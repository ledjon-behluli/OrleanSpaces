using System.Numerics;

namespace OrleanSpaces.Tuples;

public interface ISpaceTuple<T, H> : IEquatable<H>, IComparable<H>
    where T : struct
    where H : ISpaceTuple<T, H>
{
    T this[int index] { get; }
    int Length { get; }
}

public interface INumericSpaceTuple<T, H> : ISpaceTuple<T, H>
    where T : struct, INumber<T>
    where H : ISpaceTuple<T, H>
{
    T[] Fields { get; }
}

public readonly struct IntegerTuple : INumericSpaceTuple<int, IntegerTuple>
{
    public readonly int[] Fields { get; }

    public int this[int index] => Fields[index];
    public int Length { get; }

    public IntegerTuple(int[] fields)
    {
        Fields = fields;
        Length = fields.Length;
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

        return Vector.IsHardwareAccelerated ? this.SimdEquals(other) : FallbackEquals(other);
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