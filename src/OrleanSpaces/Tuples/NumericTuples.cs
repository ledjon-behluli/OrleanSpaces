using Orleans.Concurrency;

namespace OrleanSpaces.Tuples;

[Immutable]
public readonly struct SByteTuple : INumericTuple<sbyte, SByteTuple>
{
    private readonly sbyte[] fields;
    
    public sbyte this[int index] => fields[index];
    public int Length { get; }

    Span<sbyte> INumericTuple<sbyte, SByteTuple>.Span => fields.AsSpan();

    public SByteTuple(sbyte[] fields)
    {
        this.fields = fields;
        Length = fields.Length;
    }

    public static bool operator ==(SByteTuple left, SByteTuple right) => left.Equals(right);
    public static bool operator !=(SByteTuple left, SByteTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is SByteTuple tuple && Equals(tuple);
    public bool Equals(SByteTuple other) => Extensions.Equals(this, other);

    public int CompareTo(SByteTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();

    public override string ToString() => $"({string.Join(", ", fields)})";
}

[Immutable]
public readonly struct ByteTuple : INumericTuple<byte, ByteTuple>
{
    private readonly byte[] fields;

    public byte this[int index] => fields[index];
    public int Length { get; }

    Span<byte> INumericTuple<byte, ByteTuple>.Span => fields.AsSpan();

    public ByteTuple(byte[] fields)
    {
        this.fields = fields;
        Length = fields.Length;
    }

    public static bool operator ==(ByteTuple left, ByteTuple right) => left.Equals(right);
    public static bool operator !=(ByteTuple left, ByteTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is ByteTuple tuple && Equals(tuple);
    public bool Equals(ByteTuple other) => Extensions.Equals(this, other);

    public int CompareTo(ByteTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();

    public override string ToString() => $"({string.Join(", ", fields)})";
}

[Immutable]
public readonly struct ShortTuple : INumericTuple<short, ShortTuple>
{
    private readonly short[] fields;

    public short this[int index] => fields[index];
    public int Length { get; }

    Span<short> INumericTuple<short, ShortTuple>.Span => fields.AsSpan();

    public ShortTuple(short[] fields)
    {
        this.fields = fields;
        Length = fields.Length;
    }

    public static bool operator ==(ShortTuple left, ShortTuple right) => left.Equals(right);
    public static bool operator !=(ShortTuple left, ShortTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is ShortTuple tuple && Equals(tuple);
    public bool Equals(ShortTuple other) => Extensions.Equals(this, other);

    public int CompareTo(ShortTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();

    public override string ToString() => $"({string.Join(", ", fields)})";
}

[Immutable]
public readonly struct UShortTuple : INumericTuple<ushort, UShortTuple>
{
    private readonly ushort[] fields;

    public ushort this[int index] => fields[index];
    public int Length { get; }

    Span<ushort> INumericTuple<ushort, UShortTuple>.Span => fields.AsSpan();

    public UShortTuple(ushort[] fields)
    {
        this.fields = fields;
        Length = fields.Length;
    }

    public static bool operator ==(UShortTuple left, UShortTuple right) => left.Equals(right);
    public static bool operator !=(UShortTuple left, UShortTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is UShortTuple tuple && Equals(tuple);
    public bool Equals(UShortTuple other) => Extensions.Equals(this, other);

    public int CompareTo(UShortTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();

    public override string ToString() => $"({string.Join(", ", fields)})";
}

[Immutable]
public readonly struct IntTuple : INumericTuple<int, IntTuple>
{
    private readonly int[] fields;

    public int this[int index] => fields[index];
    public int Length { get; }

    Span<int> INumericTuple<int, IntTuple>.Span => fields.AsSpan();

    public IntTuple(int[] fields)
    {
        this.fields = fields;
        Length = fields.Length;
    }

    public static bool operator ==(IntTuple left, IntTuple right) => left.Equals(right);
    public static bool operator !=(IntTuple left, IntTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is IntTuple tuple && Equals(tuple);
    public bool Equals(IntTuple other) => Extensions.Equals(this, other);

    public int CompareTo(IntTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();

    public override string ToString() => $"({string.Join(", ", fields)})";
}

[Immutable]
public readonly struct UIntTuple : INumericTuple<uint, UIntTuple>
{
    private readonly uint[] fields;

    public uint this[int index] => fields[index];
    public int Length { get; }

    Span<uint> INumericTuple<uint, UIntTuple>.Span => fields.AsSpan();

    public UIntTuple(uint[] fields)
    {
        this.fields = fields;
        Length = fields.Length;
    }

    public static bool operator ==(UIntTuple left, UIntTuple right) => left.Equals(right);
    public static bool operator !=(UIntTuple left, UIntTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is UIntTuple tuple && Equals(tuple);
    public bool Equals(UIntTuple other) => Extensions.Equals(this, other);

    public int CompareTo(UIntTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();

    public override string ToString() => $"({string.Join(", ", fields)})";
}

[Immutable]
public readonly struct LongTuple : INumericTuple<long, LongTuple>
{
    private readonly long[] fields;

    public long this[int index] => fields[index];
    public int Length { get; }

    Span<long> INumericTuple<long, LongTuple>.Span => fields.AsSpan();

    public LongTuple(long[] fields)
    {
        this.fields = fields;
        Length = fields.Length;
    }

    public static bool operator ==(LongTuple left, LongTuple right) => left.Equals(right);
    public static bool operator !=(LongTuple left, LongTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is LongTuple tuple && Equals(tuple);
    public bool Equals(LongTuple other) => Extensions.Equals(this, other);

    public int CompareTo(LongTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();

    public override string ToString() => $"({string.Join(", ", fields)})";
}

[Immutable]
public readonly struct ULongTuple : INumericTuple<ulong, ULongTuple>
{
    private readonly ulong[] fields;

    public ulong this[int index] => fields[index];
    public int Length { get; }

    Span<ulong> INumericTuple<ulong, ULongTuple>.Span => fields.AsSpan();

    public ULongTuple(ulong[] fields)
    {
        this.fields = fields;
        Length = fields.Length;
    }

    public static bool operator ==(ULongTuple left, ULongTuple right) => left.Equals(right);
    public static bool operator !=(ULongTuple left, ULongTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is ULongTuple tuple && Equals(tuple);
    public bool Equals(ULongTuple other) => Extensions.Equals(this, other);

    public int CompareTo(ULongTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();

    public override string ToString() => $"({string.Join(", ", fields)})";
}

[Immutable]
public readonly struct FloatTuple : INumericTuple<float, FloatTuple>
{
    private readonly float[] fields;

    public float this[int index] => fields[index];
    public int Length { get; }

    Span<float> INumericTuple<float, FloatTuple>.Span => fields.AsSpan();

    public FloatTuple(float[] fields)
    {
        this.fields = fields;
        Length = fields.Length;
    }

    public static bool operator ==(FloatTuple left, FloatTuple right) => left.Equals(right);
    public static bool operator !=(FloatTuple left, FloatTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is FloatTuple tuple && Equals(tuple);
    public bool Equals(FloatTuple other) => Extensions.Equals(this, other);

    public int CompareTo(FloatTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();

    public override string ToString() => $"({string.Join(", ", fields)})";
}

[Immutable]
public readonly struct DoubleTuple : INumericTuple<double, DoubleTuple>
{
    private readonly double[] fields;

    public double this[int index] => fields[index];
    public int Length { get; }

    Span<double> INumericTuple<double, DoubleTuple>.Span => fields.AsSpan();

    public DoubleTuple(double[] fields)
    {
        this.fields = fields;
        Length = fields.Length;
    }

    public static bool operator ==(DoubleTuple left, DoubleTuple right) => left.Equals(right);
    public static bool operator !=(DoubleTuple left, DoubleTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is DoubleTuple tuple && Equals(tuple);
    public bool Equals(DoubleTuple other) => Extensions.Equals(this, other);

    public int CompareTo(DoubleTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();

    public override string ToString() => $"({string.Join(", ", fields)})";
}