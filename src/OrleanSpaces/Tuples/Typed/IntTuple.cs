﻿using Orleans.Concurrency;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct IntTuple : INumericSpaceTuple<int, IntTuple>
{
    private readonly int[] fields;

    public int this[int index] => fields[index];
    public int Length => fields.Length;

    Span<int> INumericSpaceTuple<int, IntTuple>.Fields => fields.AsSpan();

    //int ISpaceFormattable.MaxCharsWrittable => 10;
    //void ISpaceFormattable.WriteTo(Span<char> destination, int index, out int charsWritten) => fields[index].TryFormat(destination, out charsWritten);

    public IntTuple() : this(Array.Empty<int>()) { }
    public IntTuple(params int[] fields) => this.fields = fields;

    public static bool operator ==(IntTuple left, IntTuple right) => left.Equals(right);
    public static bool operator !=(IntTuple left, IntTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is IntTuple tuple && Equals(tuple);
    public bool Equals(IntTuple other) => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public int CompareTo(IntTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();

    public override string ToString() => this.ToTupleString();
}
