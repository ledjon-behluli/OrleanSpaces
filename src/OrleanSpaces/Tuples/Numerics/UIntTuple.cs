using System.Numerics;

namespace OrleanSpaces.Tuples.Numerics;

public readonly struct UIntTuple : INumericTuple<uint, UIntTuple>
{
    public readonly uint[] Fields { get; }

    public uint this[int index] => Fields[index];
    public int Length { get; }

    public UIntTuple(uint[] fields)
    {
        Fields = fields;
        Length = fields.Length;
    }

    public static bool operator ==(UIntTuple left, UIntTuple right) => left.Equals(right);
    public static bool operator !=(UIntTuple left, UIntTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is UIntTuple tuple && Equals(tuple);

    public bool Equals(UIntTuple other)
    {
        if (Length != other.Length)
        {
            return false;
        }

        return Vector.IsHardwareAccelerated ? this.SimdEquals(other) : FallbackEquals(other);
    }

    private bool FallbackEquals(UIntTuple other)
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

    public int CompareTo(UIntTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => Fields.GetHashCode();

    public override string ToString() => $"({string.Join(", ", Fields)})";
}