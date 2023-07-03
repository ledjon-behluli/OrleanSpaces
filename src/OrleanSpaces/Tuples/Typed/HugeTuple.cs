using Newtonsoft.Json;
using OrleanSpaces.Helpers;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace OrleanSpaces.Tuples.Typed;

[GenerateSerializer, Immutable]
public readonly struct HugeTuple : INumericTuple<Int128>, IEquatable<HugeTuple>
{
    [Id(0), JsonProperty] private readonly Int128[] fields;
    [JsonIgnore] public int Length => fields.Length;

    public ref readonly Int128 this[int index] => ref fields[index];

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

        return new Comparer(this, other).AllocateAndExecute(2 * Constants.ByteCount_Int128 * Length);
    }

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => TupleHelpers.ToString(fields);

    ISpaceTuple<Int128> ISpaceTuple<Int128>.Create(Int128[] fields) => new HugeTuple(fields);
    ISpaceTemplate<Int128> ISpaceTuple<Int128>.ToTemplate()
    {
        ref Int128?[] fields = ref TupleHelpers.CastAs<Int128[], Int128?[]>(in this.fields);
        return new HugeTemplate(fields);
    }

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
                value.WriteLittleEndian(destination.Slice(index, index + Constants.ByteCount_Int128)) :
                value.WriteBigEndian(destination.Slice(index, index + Constants.ByteCount_Int128));
    }
}

public readonly record struct HugeTemplate : ISpaceTemplate<Int128>
{
    private readonly Int128?[] fields;

    public ref readonly Int128? this[int index] => ref fields[index];
    public int Length => fields.Length;

    public HugeTemplate([AllowNull] params Int128?[] fields)
        => this.fields = fields == null || fields.Length == 0 ? new Int128?[1] { null } : fields;

    public bool Matches<TTuple>(TTuple tuple) where TTuple : ISpaceTuple<Int128>
        => TupleHelpers.Matches(this, tuple);

    public override string ToString() => TupleHelpers.ToString(fields);
    public ReadOnlySpan<Int128?>.Enumerator GetEnumerator() => new ReadOnlySpan<Int128?>(fields).GetEnumerator();
}
