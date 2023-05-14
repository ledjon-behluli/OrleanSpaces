using Orleans.Concurrency;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct Long2Tuple : INumericTuple<Int128, Long2Tuple>
{
    /// <summary>
    /// 
    /// </summary>
    /// <example>-170141183460469231731687303715884105728</example>
    internal const int MaxFieldCharLength = 40;

    internal const int Size = 16;  // see System.Int128 source code

    private readonly Int128[] fields;
    
    public ref readonly Int128 this[int index] => ref fields[index];
    public int Length => fields.Length;

    Span<Int128> INumericTuple<Int128, Long2Tuple>.Fields => fields.AsSpan();

    public Long2Tuple() : this(Array.Empty<Int128>()) { }
    public Long2Tuple(params Int128[] fields) => this.fields = fields;

    public static bool operator ==(Long2Tuple left, Long2Tuple right) => left.Equals(right);
    public static bool operator !=(Long2Tuple left, Long2Tuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is Long2Tuple tuple && Equals(tuple);
    public bool Equals(Long2Tuple other)
    {
        if (Length != other.Length)
        {
            return false;
        }

        if (!Vector.IsHardwareAccelerated)
        {
            return this.SequentialEquals(other);
        }

        return new Comparer(this, other).AllocateAndExecute(2 * Size * Length);
    }

    public int CompareTo(Long2Tuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => $"({string.Join(", ", fields)})";

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(MaxFieldCharLength);

    public ReadOnlySpan<Int128>.Enumerator GetEnumerator() => new ReadOnlySpan<Int128>(fields).GetEnumerator();

    readonly struct Comparer : IBufferConsumer<byte>
    {
        private readonly Long2Tuple left;
        private readonly Long2Tuple right;

        public Comparer(Long2Tuple left, Long2Tuple right)
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
                WriteTo(ref leftSpan, i, in left[i]);
                WriteTo(ref rightSpan, i, in right[i]);
            }

            return leftSpan.ParallelEquals(rightSpan);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteTo<T>(ref Span<byte> destination, int index, in T value) 
            where T : IBinaryInteger<T> => _ = BitConverter.IsLittleEndian ?
                value.TryWriteLittleEndian(destination.Slice(index, index + Size), out _) :
                value.TryWriteBigEndian(destination.Slice(index, index + Size), out _);
    }
}