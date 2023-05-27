using Orleans.Concurrency;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples.Typed;

[Immutable]
public readonly struct CharTuple : ISpaceTuple<char>, IEquatable<CharTuple>, IComparable<CharTuple>
{
    /// <summary>
    /// 
    /// </summary>
    /// <example>a</example>
    internal const int MaxFieldCharLength = 1;

    private readonly char[] fields;

    public ref readonly char this[int index] => ref fields[index];
    public int Length => fields.Length;

    public CharTuple() : this(Array.Empty<char>()) { }
    public CharTuple(params char[] fields) => this.fields = fields;

    public static bool operator ==(CharTuple left, CharTuple right) => left.Equals(right);
    public static bool operator !=(CharTuple left, CharTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is CharTuple tuple && Equals(tuple);

    public bool Equals(CharTuple other)
    {
        // Since 'char' is not a number (doesn't implement INumber<>), we are transforming it into a type which does implement INumber<>.
        // The sizeof(char) = 2 Bytes, therefor it can be represented by many number types, but the lowest possible (the one that provides the best parallelization)
        // number type that can safley represent any type of 'char', is 'ushort'. This is because the range of 'char' is U+0000 to U+FFFF.

        // In systems where 128-bit vector operations are subject to hardware acceleration, a total of 8 operations can be performed on 'ushort's
        //      128 / (2 * 8) = 128 / 16 = 8  --> means: we can compare 8 chars at the same time!

        // In systems where 256-bit vector operations are subject to hardware acceleration, a total of 16 operations can be performed on 'ushort's
        //      256 / (2 * 8) = 256 / 16 = 16 --> means: we can compare 16 chars at the same time!

        NumericMarshaller<char, ushort> marshaller = new(fields.AsSpan(), other.fields.AsSpan());
        return marshaller.TryParallelEquals(out bool result) ? result : this.SequentialEquals(other);
    }

    public int CompareTo(CharTuple other) => Length.CompareTo(other.Length);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => $"({string.Join(", ", fields)})";

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(MaxFieldCharLength);

    public ReadOnlySpan<char>.Enumerator GetEnumerator() => new ReadOnlySpan<char>(fields).GetEnumerator();
}

[Immutable]
public readonly struct SpaceChar
{
    public readonly char Value;

    internal static readonly SpaceChar Default = new();

    public SpaceChar(char value) => Value = value;

    public static implicit operator SpaceChar(char value) => new(value);
    public static implicit operator char(SpaceChar value) => value.Value;
}

[Immutable]
public readonly struct CharTemplate : ISpaceTemplate<CharTuple>
{
    private readonly SpaceChar[] fields;

    public CharTemplate([AllowNull] params SpaceChar[] fields)
        => this.fields = fields == null || fields.Length == 0 ? new SpaceChar[1] { new SpaceUnit() } : fields;
}
