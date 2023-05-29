﻿using Orleans.Concurrency;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct UHugeTuple : INumericTuple<UInt128>, IEquatable<UHugeTuple>, IComparable<UHugeTuple>
{
    private readonly UInt128[] fields;

    public ref readonly UInt128 this[int index] => ref fields[index];
    public int Length => fields.Length;

    Span<UInt128> INumericTuple<UInt128>.Fields => fields.AsSpan();

    public UHugeTuple() : this(Array.Empty<UInt128>()) { }
    public UHugeTuple(params UInt128[] fields) => this.fields = fields;

    public static bool operator ==(UHugeTuple left, UHugeTuple right) => left.Equals(right);
    public static bool operator !=(UHugeTuple left, UHugeTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is UHugeTuple tuple && Equals(tuple);
    public bool Equals(UHugeTuple other)
    {
        if (Length != other.Length)
        {
            return false;
        }

        if (!Vector.IsHardwareAccelerated)
        {
            return this.SequentialEquals(other);
        }

        return new Comparer(this, other).AllocateAndExecute(2 * Constants.ByteCount_UInt128 * Length);
    }

    public int CompareTo(UHugeTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => $"({string.Join(", ", fields)})";

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(Constants.MaxFieldCharLength_UHuge);

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

[Immutable]
public readonly struct UHugeTemplate : ISpaceTemplate<UInt128>
{
    private readonly UInt128?[] fields;

    public ref readonly UInt128? this[int index] => ref fields[index];
    public int Length => fields.Length;

    public UHugeTemplate([AllowNull] params UInt128?[] fields)
        => this.fields = fields == null || fields.Length == 0 ? new UInt128?[1] { null } : fields;

    public bool Matches<TTuple>(TTuple tuple) where TTuple : ISpaceTuple<UInt128>
        => Helpers.Matches(this, tuple);

    ISpaceTuple<UInt128> ISpaceTemplate<UInt128>.Create(UInt128[] fields) => new UHugeTuple(fields);
}
