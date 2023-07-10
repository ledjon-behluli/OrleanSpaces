using Newtonsoft.Json;
using OrleanSpaces.Helpers;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples.Specialized;

[GenerateSerializer, Immutable]
public readonly struct CharTuple :
    IEquatable<CharTuple>,
    ISpaceTuple<char>, 
    ISpaceFactory<char, CharTuple>,
    ISpaceConvertible<char, CharTemplate>
{
    [Id(0), JsonProperty] private readonly char[] fields;
    [JsonIgnore] public int Length => fields.Length;

    public ref readonly char this[int index] => ref fields[index];

    public CharTuple() => fields = Array.Empty<char>();
    public CharTuple([AllowNull] params char[] fields)
        => this.fields = fields is null ? Array.Empty<char>() : fields;

    public static bool operator ==(CharTuple left, CharTuple right) => left.Equals(right);
    public static bool operator !=(CharTuple left, CharTuple right) => !(left == right);

    public CharTemplate ToTemplate()
    {
        int length = Length;
        char?[] fields = new char?[length];

        for (int i = 0; i < length; i++)
        {
            fields[i] = this[i];
        }

        return new CharTemplate(fields);
    }

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

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => SpaceHelpers.ToString(fields);

    static CharTuple ISpaceFactory<char, CharTuple>.Create(char[] fields) => new(fields);

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(Constants.MaxFieldCharLength_Char);
    public ReadOnlySpan<char>.Enumerator GetEnumerator() => new ReadOnlySpan<char>(fields).GetEnumerator();
}

public readonly record struct CharTemplate : ISpaceTemplate<char>, ISpaceMatchable<char, CharTuple>
{
    private readonly char?[] fields;

    public ref readonly char? this[int index] => ref fields[index];
    public int Length => fields.Length;

    public CharTemplate() => fields = Array.Empty<char?>();
    public CharTemplate([AllowNull] params char?[] fields) => this.fields = fields is null ? Array.Empty<char?>() : fields;

    public bool Matches(CharTuple tuple) => SpaceHelpers.Matches<char, CharTuple>(this, tuple);

    public override string ToString() => SpaceHelpers.ToString(fields);
    public ReadOnlySpan<char?>.Enumerator GetEnumerator() => new ReadOnlySpan<char?>(fields).GetEnumerator();
}
