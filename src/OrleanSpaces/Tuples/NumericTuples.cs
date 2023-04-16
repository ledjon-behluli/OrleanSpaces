using Orleans.Concurrency;
using System.Runtime.Intrinsics;

namespace OrleanSpaces.Tuples;

[Immutable]
public readonly struct SByteTuple : INumericTuple<sbyte, SByteTuple>
{
    private readonly sbyte[] fields;
    
    public sbyte this[int index] => fields[index];
    public int Length => fields.Length;

    Span<sbyte> INumericTuple<sbyte, SByteTuple>.Data => fields.AsSpan();

    public SByteTuple() : this(Array.Empty<sbyte>()) { }
    public SByteTuple(sbyte[] fields) => this.fields = fields;

    public static bool operator ==(SByteTuple left, SByteTuple right) => left.Equals(right);
    public static bool operator !=(SByteTuple left, SByteTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is SByteTuple tuple && Equals(tuple);
    public bool Equals(SByteTuple other) => Extensions.SimdEquals(this, other);

    public int CompareTo(SByteTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();

    public override string ToString() => $"({string.Join(", ", fields)})";
}

[Immutable]
public readonly struct ByteTuple : INumericTuple<byte, ByteTuple>
{
    private readonly byte[] fields;

    public byte this[int index] => fields[index];
    public int Length => fields.Length;

    Span<byte> INumericTuple<byte, ByteTuple>.Data => fields.AsSpan();

    public ByteTuple() : this(Array.Empty<byte>()) { }
    public ByteTuple(byte[] fields) => this.fields = fields;

    public static bool operator ==(ByteTuple left, ByteTuple right) => left.Equals(right);
    public static bool operator !=(ByteTuple left, ByteTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is ByteTuple tuple && Equals(tuple);
    public bool Equals(ByteTuple other) => Extensions.SimdEquals(this, other);

    public int CompareTo(ByteTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();

    public override string ToString() => $"({string.Join(", ", fields)})";
}

[Immutable]
public readonly struct ShortTuple : INumericTuple<short, ShortTuple>
{
    private readonly short[] fields;

    public short this[int index] => fields[index];
    public int Length => fields.Length;

    Span<short> INumericTuple<short, ShortTuple>.Data => fields.AsSpan();

    public ShortTuple() : this(Array.Empty<short>()) { }
    public ShortTuple(short[] fields) => this.fields = fields;

    public static bool operator ==(ShortTuple left, ShortTuple right) => left.Equals(right);
    public static bool operator !=(ShortTuple left, ShortTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is ShortTuple tuple && Equals(tuple);
    public bool Equals(ShortTuple other) => Extensions.SimdEquals(this, other);

    public int CompareTo(ShortTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();

    public override string ToString() => $"({string.Join(", ", fields)})";
}

[Immutable]
public readonly struct UShortTuple : INumericTuple<ushort, UShortTuple>
{
    private readonly ushort[] fields;

    public ushort this[int index] => fields[index];
    public int Length => fields.Length;

    Span<ushort> INumericTuple<ushort, UShortTuple>.Data => fields.AsSpan();

    public UShortTuple() : this(Array.Empty<ushort>()) { }
    public UShortTuple(ushort[] fields) => this.fields = fields;

    public static bool operator ==(UShortTuple left, UShortTuple right) => left.Equals(right);
    public static bool operator !=(UShortTuple left, UShortTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is UShortTuple tuple && Equals(tuple);
    public bool Equals(UShortTuple other) => Extensions.SimdEquals(this, other);

    public int CompareTo(UShortTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();

    public override string ToString() => $"({string.Join(", ", fields)})";
}

[Immutable]
public readonly struct IntTuple : INumericTuple<int, IntTuple>
{
    private readonly int[] fields;

    public int this[int index] => fields[index];
    public int Length => fields.Length;

    Span<int> INumericTuple<int, IntTuple>.Data => fields.AsSpan();

    public IntTuple() : this(Array.Empty<int>()) { }
    public IntTuple(int[] fields) => this.fields = fields;

    public static bool operator ==(IntTuple left, IntTuple right) => left.Equals(right);
    public static bool operator !=(IntTuple left, IntTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is IntTuple tuple && Equals(tuple);
    public bool Equals(IntTuple other) => Extensions.SimdEquals(this, other);

    public int CompareTo(IntTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();

    public override string ToString() => $"({string.Join(", ", fields)})";
}

[Immutable]
public readonly struct UIntTuple : INumericTuple<uint, UIntTuple>
{
    private readonly uint[] fields;

    public uint this[int index] => fields[index];
    public int Length => fields.Length;

    Span<uint> INumericTuple<uint, UIntTuple>.Data => fields.AsSpan();

    public UIntTuple() : this(Array.Empty<uint>()) { }
    public UIntTuple(uint[] fields) => this.fields = fields;

    public static bool operator ==(UIntTuple left, UIntTuple right) => left.Equals(right);
    public static bool operator !=(UIntTuple left, UIntTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is UIntTuple tuple && Equals(tuple);
    public bool Equals(UIntTuple other) => Extensions.SimdEquals(this, other);

    public int CompareTo(UIntTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();

    public override string ToString() => $"({string.Join(", ", fields)})";
}

[Immutable]
public readonly struct LongTuple : INumericTuple<long, LongTuple>
{
    private readonly long[] fields;

    public long this[int index] => fields[index];
    public int Length => fields.Length;

    Span<long> INumericTuple<long, LongTuple>.Data => fields.AsSpan();

    public LongTuple() : this(Array.Empty<long>()) { }
    public LongTuple(long[] fields) => this.fields = fields;

    public static bool operator ==(LongTuple left, LongTuple right) => left.Equals(right);
    public static bool operator !=(LongTuple left, LongTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is LongTuple tuple && Equals(tuple);
    public bool Equals(LongTuple other) => Extensions.SimdEquals(this, other);

    public int CompareTo(LongTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();

    public override string ToString() => $"({string.Join(", ", fields)})";
}

[Immutable]
public readonly struct ULongTuple : INumericTuple<ulong, ULongTuple>
{
    private readonly ulong[] fields;

    public ulong this[int index] => fields[index];
    public int Length => fields.Length;

    Span<ulong> INumericTuple<ulong, ULongTuple>.Data => fields.AsSpan();

    public ULongTuple() : this(Array.Empty<ulong>()) { }
    public ULongTuple(ulong[] fields) => this.fields = fields;

    public static bool operator ==(ULongTuple left, ULongTuple right) => left.Equals(right);
    public static bool operator !=(ULongTuple left, ULongTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is ULongTuple tuple && Equals(tuple);
    public bool Equals(ULongTuple other) => Extensions.SimdEquals(this, other);

    public int CompareTo(ULongTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();

    public override string ToString() => $"({string.Join(", ", fields)})";
}

[Immutable]
public readonly struct FloatTuple : INumericTuple<float, FloatTuple>
{
    private readonly float[] fields;

    public float this[int index] => fields[index];
    public int Length => fields.Length;

    Span<float> INumericTuple<float, FloatTuple>.Data => fields.AsSpan();

    public FloatTuple() : this(Array.Empty<float>()) { }
    public FloatTuple(float[] fields) => this.fields = fields;

    public static bool operator ==(FloatTuple left, FloatTuple right) => left.Equals(right);
    public static bool operator !=(FloatTuple left, FloatTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is FloatTuple tuple && Equals(tuple);
    public bool Equals(FloatTuple other) => Extensions.SimdEquals(this, other);

    public int CompareTo(FloatTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();

    public override string ToString() => $"({string.Join(", ", fields)})";
}

[Immutable]
public readonly struct DoubleTuple : INumericTuple<double, DoubleTuple>
{
    private readonly double[] fields;

    public double this[int index] => fields[index];
    public int Length => fields.Length;

    Span<double> INumericTuple<double, DoubleTuple>.Data => fields.AsSpan();

    public DoubleTuple() : this(Array.Empty<double>()) { }
    public DoubleTuple(double[] fields) => this.fields = fields;

    public static bool operator ==(DoubleTuple left, DoubleTuple right) => left.Equals(right);
    public static bool operator !=(DoubleTuple left, DoubleTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is DoubleTuple tuple && Equals(tuple);
    public bool Equals(DoubleTuple other) => Extensions.SimdEquals(this, other);

    public int CompareTo(DoubleTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();

    public override string ToString() => $"({string.Join(", ", fields)})";
}

[Immutable]
public readonly struct GuidTuple : ISpaceTuple<Guid, GuidTuple>
{
    private readonly Guid[] fields;

    public Guid this[int index] => fields[index];
    public int Length => fields.Length;

    public GuidTuple() : this(Array.Empty<Guid>()) { }
    public GuidTuple(Guid[] fields) => this.fields = fields;

    public static bool operator ==(GuidTuple left, GuidTuple right) => left.Equals(right);
    public static bool operator !=(GuidTuple left, GuidTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is GuidTuple tuple && Equals(tuple);

    public bool Equals(GuidTuple other)
    {
        if (Length != other.Length)
        {
            return false;
        }

        if (Vector128.IsHardwareAccelerated)
        {
            for (int i = 0; i < Length; i++)
            {
                ref Vector128<byte> vLeft = ref Extensions.AsRef<Guid, Vector128<byte>>(in fields[i]);
                ref Vector128<byte> vRight = ref Extensions.AsRef<Guid, Vector128<byte>>(in other.fields[i]);

                if (vLeft != vRight)  //todo: test me!
                {
                    return false;
                }
            }

            return true;
        }

        return Extensions.FallbackEquals(this, other);
    }

    public int CompareTo(GuidTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();

    public override string ToString() => $"({string.Join(", ", fields)})";
}