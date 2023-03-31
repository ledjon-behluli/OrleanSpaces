using System.Numerics;

namespace OrleanSpaces.Tuples.Numerics;

public readonly struct IntTuple : INumericTuple<int, IntTuple>
{
    public readonly int[] Fields { get; }

    public int this[int index] => Fields[index];
    public int Length { get; }

    public IntTuple(int[] fields)
    {
        Fields = fields;
        Length = fields.Length;
    }

    public static bool operator ==(IntTuple left, IntTuple right) => left.Equals(right);
    public static bool operator !=(IntTuple left, IntTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is IntTuple tuple && Equals(tuple);

    public bool Equals(IntTuple other)
    {
        if (Length != other.Length)
        {
            return false;
        }

        return Vector.IsHardwareAccelerated ? this.SimdEquals(other) : FallbackEquals(other);
    }

    private bool FallbackEquals(IntTuple other)
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

    public int CompareTo(IntTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => Fields.GetHashCode();

    public override string ToString() => $"({string.Join(", ", Fields)})";
}