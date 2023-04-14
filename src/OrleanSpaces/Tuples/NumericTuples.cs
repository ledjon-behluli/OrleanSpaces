namespace OrleanSpaces.Tuples;

public readonly struct SByteTuple : INumericTuple<sbyte, LongTuple>
{

}

public readonly struct ByteTuple : INumericTuple<byte, LongTuple>
{

}

public readonly struct ShortTuple : INumericTuple<short, LongTuple>
{

}

public readonly struct UShortTuple : INumericTuple<ushort, LongTuple>
{

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

}

public readonly struct ULongTuple : INumericTuple<ulong, LongTuple>
{

}

public readonly struct FloatTuple : INumericTuple<float, LongTuple>
{

}

public readonly struct DoubleTuple : INumericTuple<double, LongTuple>
{

}
