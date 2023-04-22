using Orleans.Concurrency;
using System.Runtime.Intrinsics;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct CharTuple : ISpaceTuple<char, CharTuple>, IFieldFormater<char>
{
    private readonly char[] fields;

    public char this[int index] => fields[index];
    public int Length => fields.Length;

    public CharTuple() : this(Array.Empty<char>()) { }
    public CharTuple(params char[] fields) => this.fields = fields;

    public static bool operator ==(CharTuple left, CharTuple right) => left.Equals(right);
    public static bool operator !=(CharTuple left, CharTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is CharTuple tuple && Equals(tuple);

    public bool Equals(CharTuple other) => throw new NotImplementedException();

    public int CompareTo(CharTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();

    public bool TryFormat(Span<char> destination, out int charsWritten)
        => this.TryFormatTuple(destination, out charsWritten);

    static int IFieldFormater<char>.MaxCharsWrittable => 11;

    static bool IFieldFormater<char>.TryFormat(char field, Span<char> destination, out int charsWritten)
        => throw new NotImplementedException();  //TODO: Implement

    public override string ToString() => $"({string.Join(", ", fields)})";
}