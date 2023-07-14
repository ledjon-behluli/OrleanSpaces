using Newtonsoft.Json;
using OrleanSpaces.Helpers;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace OrleanSpaces.Tuples.Specialized;

/// <summary>
/// Represents a tuple which has <see cref="Int128"/> field types only.
/// </summary>
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

    /// <summary>
    /// Default constructor which instantiates an empty tuple. 
    /// </summary>
    public HugeTuple() => fields = Array.Empty<Int128>();

    /// <summary>
    /// Main constructor which instantiates a non-empty tuple, when at least one field is supplied, otherwise an empty tuple is instantiated.
    /// </summary>
    /// <param name="fields">The elements of this tuple.</param>
    public HugeTuple([AllowNull] params Int128[] fields)
        => this.fields = fields is null ? Array.Empty<Int128>() : fields;

    /// <summary>
    /// Returns a <see cref="HugeTemplate"/> with the same fields as <see langword="this"/>.
    /// </summary>
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

/// <summary>
/// Represents a template which has <see cref="Int128"/> field types only.
/// </summary>
public readonly record struct HugeTemplate :
    IEquatable<HugeTemplate>,
    ISpaceTemplate<Int128>, 
    ISpaceMatchable<Int128, HugeTuple>
{
    private readonly Int128?[] fields;

    public ref readonly Int128? this[int index] => ref fields[index];
    public int Length => fields?.Length ?? 0;

    /// <summary>
    /// Default constructor which instantiates an empty template. 
    /// </summary>
    public HugeTemplate() => fields = Array.Empty<Int128?>();

    /// <summary>
    /// Main constructor which instantiates a non-empty template.
    /// </summary>
    /// <param name="fields">The elements of this template.</param>
    /// <remarks><i>If <paramref name="fields"/> is <see langword="null"/>, a template with a single <see langword="null"/> field is returned.</i></remarks>
    public HugeTemplate([AllowNull] params Int128?[] fields)
        => this.fields = fields is null ? new Int128?[1] { null } : fields;

    /// <summary>
    /// Determines whether <see langword="this"/> matches the specified <paramref name="tuple"/>.
    /// </summary>
    /// <param name="tuple">A tuple to be matched by <see langword="this"/>.</param>
    /// <returns><see langword="true"/>, if <see langword="this"/> and <paramref name="tuple"/> share the same number of fields, and all of them match on the index and value 
    /// (<i>except when any field of <see langword="this"/> is of type <see langword="null"/></i>); otherwise, <see langword="false"/>.</returns>
    public bool Matches(HugeTuple tuple) => this.Matches<Int128, HugeTuple>(tuple);
    public bool Equals(HugeTemplate other) => this.SequentialEquals(other);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => SpaceHelpers.ToString(fields);

    public ReadOnlySpan<Int128?>.Enumerator GetEnumerator() => new ReadOnlySpan<Int128?>(fields).GetEnumerator();
}
