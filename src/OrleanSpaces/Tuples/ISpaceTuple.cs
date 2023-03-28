using System;
using System.Numerics;
using System.Runtime.CompilerServices;

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
    T[] Fields { get; }
}

public readonly struct IntegerTuple : ITypedSpaceTuple<int, IntegerTuple>
{
    public readonly int[] Fields { get; }

    public int this[int index] => Fields[index];
    public int Length { get; }

    public IntegerTuple(int[] fields)
    {
        Length = fields.Length;
        Fields = fields;
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
            Vector<int> vector1 = BuildVector(Fields);
            Vector<int> vector2 = BuildVector(other.Fields);

            return Vector.EqualsAll(vector1, vector2);
        }

        return FallbackEquals(other);
    }

    // TODO: Handle case where 'fields' is greater than 'Vector<int>.Count'
    private static Vector<int> BuildVector(int[] fields)
    {
        Vector<int> fieldsVector = Vector<int>.Zero;

        int diff = Vector<int>.Count - fields.Length;
        if (diff == 0)
        {
            return new(fields);
        }

        Span<int> current = new(fields);
        Span<int> target = stackalloc int[Vector<int>.Count];

        current.CopyTo(target);

        Span<int> paddings = stackalloc int[diff];
        paddings.Fill(0);

        paddings.CopyTo(target.Slice(current.Length));

        return new(target);
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