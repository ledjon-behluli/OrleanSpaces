﻿using Orleans.Concurrency;
using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct HugeTuple : INumericTuple<Int128, HugeTuple>
{
    /// <summary>
    /// 
    /// </summary>
    /// <example>-170141183460469231731687303715884105728</example>
    internal const int MaxFieldCharLength = 40;

    /// <summary>
    /// Maximum value can be represented by this number of bytes.
    /// </summary>
    internal const int ByteCount = 16;

    private readonly Int128[] fields;
    
    public ref readonly Int128 this[int index] => ref fields[index];
    public int Length => fields.Length;

    Span<Int128> INumericTuple<Int128, HugeTuple>.Fields => fields.AsSpan();

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

        return new Comparer(this, other).AllocateAndExecute(2 * Size * Length);
    }

    public int CompareTo(HugeTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => $"({string.Join(", ", fields)})";

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(MaxFieldCharLength);

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
                WriteTo(ref leftSpan, in left[i], i);
                WriteTo(ref rightSpan, in right[i], i);
            }

            return leftSpan.ParallelEquals(rightSpan);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteTo<T>(ref Span<byte> destination, int index, in T value)
            where T : IBinaryInteger<T> => 
            _ = BitConverter.IsLittleEndian ?
                value.WriteLittleEndian(destination.Slice(index, index + ByteCount)) :
                value.WriteBigEndian(destination.Slice(index, index + ByteCount));
    }
}


[Immutable]
public readonly struct UHugeTuple : INumericTuple<UInt128, UHugeTuple>
{
    /// <summary>
    /// 
    /// </summary>
    /// <example>170141183460469231731687303715884105728</example>
    internal const int MaxFieldCharLength = 39;

    /// <summary>
    /// Maximum value can be represented by this number of bytes.
    /// </summary>
    internal const int ByteCount = 16;

    private readonly UInt128[] fields;

    public ref readonly UInt128 this[int index] => ref fields[index];
    public int Length => fields.Length;

    Span<UInt128> INumericTuple<UInt128, UHugeTuple>.Fields => fields.AsSpan();

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

        return new Comparer(this, other).AllocateAndExecute(2 * ByteCount * Length);
    }

    public int CompareTo(UHugeTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => $"({string.Join(", ", fields)})";

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(MaxFieldCharLength);

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
                WriteTo(ref leftSpan, in left[i], i);
                WriteTo(ref rightSpan, in right[i], i);
            }

            return leftSpan.ParallelEquals(rightSpan);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteTo<T>(ref Span<byte> destination, int index, in T value)
            where T : IBinaryInteger<T> => 
            _ = BitConverter.IsLittleEndian ?
                value.WriteLittleEndian(destination.Slice(index, index + ByteCount)) :
                value.WriteBigEndian(destination.Slice(index, index + ByteCount));
    }
}