using Orleans.Concurrency;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct HugeTuple : INumericTuple<Int128>, IEquatable<HugeTuple>, IComparable<HugeTuple>
{
    private readonly Int128[] fields;
    
    public ref readonly Int128 this[int index] => ref fields[index];
    public int Length => fields.Length;

    Span<Int128> INumericTuple<Int128>.Fields => fields.AsSpan();

    public HugeTuple() : this(Array.Empty<Int128>()) { }
    public HugeTuple(params Int128[] fields) => this.fields = fields;

    public static bool operator ==(HugeTuple left, HugeTuple right) => left.Equals(right);
    public static bool operator !=(HugeTuple left, HugeTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is HugeTuple tuple && Equals(tuple);
    public bool Equals(HugeTuple other)
    {
        if (Length != other.Length)
        {
            return false;
        }

        if (!Vector.IsHardwareAccelerated)
        {
            return this.SequentialEquals(other);
        }

        return new Comparer(this, other).AllocateAndExecute(32 * Length); // 32 because 2 x 16, where 16 is the maximum value can be represented by this number of bytes.
    }

    public int CompareTo(HugeTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => $"({string.Join(", ", fields)})";

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(Constants.MaxFieldCharLength_Huge);

    public ReadOnlySpan<Int128>.Enumerator GetEnumerator() => new ReadOnlySpan<Int128>(fields).GetEnumerator();

    readonly struct Comparer : IBufferConsumer<byte>
    {
        private readonly HugeTuple left;
        private readonly HugeTuple right;

        public Comparer(HugeTuple left, HugeTuple right)
        {
            this.left = left;
            this.right = right;
        }

        public bool Consume(ref Span<byte> buffer)
        {
            int tupleLength = left.Length;
            int bufferHalfLength = buffer.Length / 2;

            Span<byte> leftSpan = buffer[..bufferHalfLength];
            Span<byte> rightSpan = buffer[bufferHalfLength..];

            for (int i = 0; i < tupleLength; i++)
            {
                WriteTo(i, ref leftSpan, in left[i]);
                WriteTo(i, ref rightSpan, in right[i]);
            }

            return leftSpan.ParallelEquals(rightSpan);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteTo<T>(int index, ref Span<byte> destination, in T value)
            where T : IBinaryInteger<T> => 
            _ = BitConverter.IsLittleEndian ?
                value.WriteLittleEndian(destination.Slice(index, index + ByteCount)) :
                value.WriteBigEndian(destination.Slice(index, index + ByteCount));
    }
}

[Immutable]
public readonly struct SpaceHuge
{
    public readonly Int128 Value;

    internal static readonly SpaceHuge Default = new();

    public SpaceHuge(Int128 value) => Value = value;

    public static implicit operator SpaceHuge(Int128 value) => new(value);
    public static implicit operator Int128(SpaceHuge value) => value.Value;
}

[Immutable]
public readonly struct HugeTemplate : ISpaceTemplate<HugeTuple>
{
    private readonly SpaceHuge[] fields;

    public HugeTemplate([AllowNull] params SpaceHuge[] fields)
        => this.fields = fields == null || fields.Length == 0 ? new SpaceHuge[1] { new SpaceUnit() } : fields;
}
