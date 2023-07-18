using Newtonsoft.Json;
using OrleanSpaces.Helpers;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace OrleanSpaces.Tuples.Specialized;

/// <summary>
/// Represents a tuple which has <see cref="UInt128"/> field types only.
/// </summary>
[GenerateSerializer, Immutable]
public readonly record struct UHugeTuple :
    IEquatable<UHugeTuple>,
    INumericTuple<UInt128>, 
    ISpaceFactory<UInt128, UHugeTuple>,
    ISpaceConvertible<UInt128, UHugeTemplate>
{
    [Id(0), JsonProperty] private readonly UInt128[] fields;
    [JsonIgnore] public int Length => fields?.Length ?? 0;
    [JsonIgnore] public bool IsEmpty => Length == 0;

    public ref readonly UInt128 this[int index] => ref fields[index];

    Span<UInt128> INumericTuple<UInt128>.Fields => fields.AsSpan();

    /// <summary>
    /// Initializes an empty tuple.
    /// </summary>
    public UHugeTuple() : this(null) { }

    /// <summary>
    /// If <paramref name="fields"/> is <see langword="null"/>, initializes an empty tuple.
    /// </summary>
    /// <param name="fields">The elements of this tuple.</param>
    public UHugeTuple([AllowNull] params UInt128[] fields)
        => this.fields = fields is null ? Array.Empty<UInt128>() : fields;

    /// <summary>
    /// Returns a <see cref="UHugeTemplate"/> with the same fields as <see langword="this"/>.
    /// </summary>
    /// <remarks><i>If <see cref="Length"/> is 0, than default <see cref="UHugeTemplate"/> is created.</i></remarks>
    public UHugeTemplate ToTemplate()
    {
        int length = Length;
        UInt128?[] fields = new UInt128?[length];

        for (int i = 0; i < length; i++)
        {
            fields[i] = this[i];
        }

        return new UHugeTemplate(fields);
    }

    public bool Equals(UHugeTuple other)
    {
        if (Length != other.Length)
        {
            return false;
        }

        if (!Vector.IsHardwareAccelerated)
        {
            return this.SequentialEquals<UInt128, UHugeTuple>(other);
        }

        return new Comparer(this, other).AllocateAndExecute<byte, Comparer>(2 * Constants.ByteCount_UInt128 * Length);
    }

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => TupleHelpers.ToString(fields);

    static UHugeTuple ISpaceFactory<UInt128, UHugeTuple>.Create(UInt128[] fields) => new(fields);

    public ReadOnlySpan<char> AsSpan() => this.AsSpan<UInt128, UHugeTuple>(Constants.MaxFieldCharLength_UHuge);
    public ReadOnlySpan<UInt128>.Enumerator GetEnumerator() => new ReadOnlySpan<UInt128>(fields).GetEnumerator();

    readonly struct Comparer : IBufferConsumer<byte>
    {
        private readonly UHugeTuple left;
        private readonly UHugeTuple right;

        public Comparer(UHugeTuple left, UHugeTuple right)
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
                value.WriteLittleEndian(destination.Slice(index, index + Constants.ByteCount_UInt128)) :
                value.WriteBigEndian(destination.Slice(index, index + Constants.ByteCount_UInt128));
    }
}

/// <summary>
/// Represents a template which has <see cref="UInt128"/> field types only.
/// </summary>
public readonly record struct UHugeTemplate : 
    IEquatable<UHugeTemplate>,
    ISpaceTemplate<UInt128>, 
    ISpaceMatchable<UInt128, UHugeTuple>
{
    private readonly UInt128?[] fields;

    public ref readonly UInt128? this[int index] => ref fields[index];
    public int Length => fields?.Length ?? 0;

    /// <summary>
    /// Initializes a template with a single <see langword="null"/> field.
    /// </summary>
    public UHugeTemplate() : this(null) { }

    /// <summary>
    /// If <paramref name="fields"/> is <see langword="null"/>, initializes a template with a single <see langword="null"/> field.
    /// </summary>
    /// <param name="fields">The elements of this template.</param>
    public UHugeTemplate([AllowNull] params UInt128?[] fields)
        => this.fields = fields is null || fields.Length == 0 ? new UInt128?[1] { null } : fields;

    /// <summary>
    /// Determines whether <see langword="this"/> matches the specified <paramref name="tuple"/>.
    /// </summary>
    /// <param name="tuple">A tuple to be matched by <see langword="this"/>.</param>
    /// <returns><see langword="true"/>, if <see langword="this"/> and <paramref name="tuple"/> share the same number of fields, and all of them match on the index and value 
    /// (<i>except when any field of <see langword="this"/> is of type <see langword="null"/></i>); otherwise, <see langword="false"/>.</returns>
    public bool Matches(UHugeTuple tuple) => this.Matches<UInt128, UHugeTuple, UHugeTemplate>(tuple);
    public bool Equals(UHugeTemplate other) => this.SequentialEquals<UInt128, UHugeTemplate>(other);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => TemplateHelpers.ToString(fields);

    public ReadOnlySpan<UInt128?>.Enumerator GetEnumerator() => new ReadOnlySpan<UInt128?>(fields).GetEnumerator();
}
