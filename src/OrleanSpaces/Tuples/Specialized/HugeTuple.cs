using Newtonsoft.Json;
using OrleanSpaces.Helpers;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace OrleanSpaces.Tuples.Specialized;

[GenerateSerializer, Immutable]
public readonly record struct HugeTuple :
    IEquatable<HugeTuple>,
    INumericTuple<Int128>,
    ISpaceFactory<Int128, HugeTuple>,
    ISpaceConvertible<Int128, HugeTemplate>
{
    [Id(0), JsonProperty] private readonly Int128[] fields;
    [JsonIgnore] public int Length => fields?.Length ?? 0;

    public ref readonly Int128 this[int index] => ref fields[index];

    Span<Int128> INumericTuple<Int128>.Fields => fields.AsSpan();

    public HugeTuple() => fields = Array.Empty<Int128>();
    public HugeTuple([AllowNull] params Int128[] fields)
        => this.fields = fields is null ? Array.Empty<Int128>() : fields;

    public HugeTemplate ToTemplate()
    {
        int length = Length;
        Int128?[] fields = new Int128?[length];

        for (int i = 0; i < length; i++)
        {
            fields[i] = this[i];
        }

        return new HugeTemplate(fields);
    }

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
    public override string ToString() => SpaceHelpers.ToString(fields);

    static HugeTuple ISpaceFactory<Int128, HugeTuple>.Create(Int128[] fields) => new(fields);

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

public readonly record struct HugeTemplate :
    IEquatable<HugeTemplate>,
    ISpaceTemplate<Int128>, 
    ISpaceMatchable<Int128, HugeTuple>
{
    private readonly Int128?[] fields;

    public ref readonly Int128? this[int index] => ref fields[index];
    public int Length => fields?.Length ?? 0;

    public HugeTemplate() => fields = Array.Empty<Int128?>();
    public HugeTemplate([AllowNull] params Int128?[] fields)
        => this.fields = fields is null ? Array.Empty<Int128?>() : fields;

    public bool Matches(HugeTuple tuple) => this.Matches<Int128, HugeTuple>(tuple);
    public bool Equals(HugeTemplate other) => this.SequentialEquals(other);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => SpaceHelpers.ToString(fields);

    public ReadOnlySpan<Int128?>.Enumerator GetEnumerator() => new ReadOnlySpan<Int128?>(fields).GetEnumerator();
}
