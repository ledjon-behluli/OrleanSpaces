using System;

namespace OrleanSpaces.Tuples;

public readonly struct SByteTuple : INumericTuple<sbyte, SByteTuple>
{
    public readonly sbyte[] Fields { get; }

    public sbyte this[int index] => Fields[index];
    public int Length { get; }

    public SByteTuple(sbyte[] fields)
    {
        Fields = fields;
        Length = fields.Length;
    }

    public static bool operator ==(SByteTuple left, SByteTuple right) => left.Equals(right);
    public static bool operator !=(SByteTuple left, SByteTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is SByteTuple tuple && Equals(tuple);
    public bool Equals(SByteTuple other) => NumericExtensions.Equals(this, other);

    public int CompareTo(SByteTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => Fields.GetHashCode();

    public override string ToString() => $"({string.Join(", ", Fields)})";
}

public readonly struct ByteTuple : INumericTuple<byte, ByteTuple>
{
    public readonly byte[] Fields { get; }

    public byte this[int index] => Fields[index];
    public int Length { get; }

    public ByteTuple(byte[] fields)
    {
        Fields = fields;
        Length = fields.Length;
    }

    public static bool operator ==(ByteTuple left, ByteTuple right) => left.Equals(right);
    public static bool operator !=(ByteTuple left, ByteTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is ByteTuple tuple && Equals(tuple);
    public bool Equals(ByteTuple other) => NumericExtensions.Equals(this, other);

    public int CompareTo(ByteTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => Fields.GetHashCode();

    public override string ToString() => $"({string.Join(", ", Fields)})";
}

public readonly struct ShortTuple : INumericTuple<short, ShortTuple>
{
    public readonly short[] Fields { get; }

    public short this[int index] => Fields[index];
    public int Length { get; }

    public ShortTuple(short[] fields)
    {
        Fields = fields;
        Length = fields.Length;
    }

    public static bool operator ==(ShortTuple left, ShortTuple right) => left.Equals(right);
    public static bool operator !=(ShortTuple left, ShortTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is ShortTuple tuple && Equals(tuple);
    public bool Equals(ShortTuple other) => NumericExtensions.Equals(this, other);

    public int CompareTo(ShortTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => Fields.GetHashCode();

    public override string ToString() => $"({string.Join(", ", Fields)})";
}

public readonly struct UShortTuple : INumericTuple<ushort, UShortTuple>
{
    public readonly ushort[] Fields { get; }

    public ushort this[int index] => Fields[index];
    public int Length { get; }

    public UShortTuple(ushort[] fields)
    {
        Fields = fields;
        Length = fields.Length;
    }

    public static bool operator ==(UShortTuple left, UShortTuple right) => left.Equals(right);
    public static bool operator !=(UShortTuple left, UShortTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is UShortTuple tuple && Equals(tuple);
    public bool Equals(UShortTuple other) => NumericExtensions.Equals(this, other);

    public int CompareTo(UShortTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => Fields.GetHashCode();

    public override string ToString() => $"({string.Join(", ", Fields)})";
}

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
    public bool Equals(IntTuple other) => NumericExtensions.Equals(this, other);

    public int CompareTo(IntTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => Fields.GetHashCode();

    public override string ToString() => $"({string.Join(", ", Fields)})";
}

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
    public bool Equals(UIntTuple other) => NumericExtensions.Equals(this, other);

    public int CompareTo(UIntTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => Fields.GetHashCode();

    public override string ToString() => $"({string.Join(", ", Fields)})";
}

public readonly struct LongTuple : INumericTuple<long, LongTuple>
{
    public readonly long[] Fields { get; }

    public long this[int index] => Fields[index];
    public int Length { get; }

    public LongTuple(long[] fields)
    {
        Fields = fields;
        Length = fields.Length;
    }

    public static bool operator ==(LongTuple left, LongTuple right) => left.Equals(right);
    public static bool operator !=(LongTuple left, LongTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is LongTuple tuple && Equals(tuple);
    public bool Equals(LongTuple other) => NumericExtensions.Equals(this, other);

    public int CompareTo(LongTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => Fields.GetHashCode();

    public override string ToString() => $"({string.Join(", ", Fields)})";
}

public readonly struct ULongTuple : INumericTuple<ulong, ULongTuple>
{
    public readonly ulong[] Fields { get; }

    public ulong this[int index] => Fields[index];
    public int Length { get; }

    public ULongTuple(ulong[] fields)
    {
        Fields = fields;
        Length = fields.Length;
    }

    public static bool operator ==(ULongTuple left, ULongTuple right) => left.Equals(right);
    public static bool operator !=(ULongTuple left, ULongTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is ULongTuple tuple && Equals(tuple);
    public bool Equals(ULongTuple other) => NumericExtensions.Equals(this, other);

    public int CompareTo(ULongTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => Fields.GetHashCode();

    public override string ToString() => $"({string.Join(", ", Fields)})";
}

public readonly struct FloatTuple : INumericTuple<float, FloatTuple>
{
    public readonly float[] Fields { get; }

    public float this[int index] => Fields[index];
    public int Length { get; }

    public FloatTuple(float[] fields)
    {
        Fields = fields;
        Length = fields.Length;
    }

    public static bool operator ==(FloatTuple left, FloatTuple right) => left.Equals(right);
    public static bool operator !=(FloatTuple left, FloatTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is FloatTuple tuple && Equals(tuple);
    public bool Equals(FloatTuple other) => NumericExtensions.Equals(this, other);

    public int CompareTo(FloatTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => Fields.GetHashCode();

    public override string ToString() => $"({string.Join(", ", Fields)})";
}

public readonly struct DoubleTuple : INumericTuple<double, DoubleTuple>
{
    public readonly double[] Fields { get; }

    public double this[int index] => Fields[index];
    public int Length { get; }

    public DoubleTuple(double[] fields)
    {
        Fields = fields;
        Length = fields.Length;
    }

    public static bool operator ==(DoubleTuple left, DoubleTuple right) => left.Equals(right);
    public static bool operator !=(DoubleTuple left, DoubleTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is DoubleTuple tuple && Equals(tuple);
    public bool Equals(DoubleTuple other) => NumericExtensions.Equals(this, other);

    public int CompareTo(DoubleTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => Fields.GetHashCode();

    public override string ToString() => $"({string.Join(", ", Fields)})";
}
