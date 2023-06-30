using Newtonsoft.Json;
using OrleanSpaces.Helpers;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples.Typed;

[GenerateSerializer, Immutable]
public readonly struct BoolTuple : ISpaceTuple<bool>, IEquatable<BoolTuple>
{
    [Id(0), JsonProperty] private readonly bool[] fields;
    [JsonProperty] public int Length => fields.Length;

    public ref readonly bool this[int index] => ref fields[index];

    public BoolTuple() : this(Array.Empty<bool>()) { }
    public BoolTuple(params bool[] fields) => this.fields = fields;

    public static bool operator ==(BoolTuple left, BoolTuple right) => left.Equals(right);
    public static bool operator !=(BoolTuple left, BoolTuple right) => !(left == right);

    public override bool Equals(object? obj) => obj is BoolTuple tuple && Equals(tuple);

    public bool Equals(BoolTuple other)
    {
        NumericMarshaller<bool, byte> marshaller = new(fields.AsSpan(), other.fields.AsSpan());
        return marshaller.TryParallelEquals(out bool result) ? result : this.SequentialEquals(other);
    }

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => TupleHelpers.ToString(fields);

    public ReadOnlySpan<bool>.Enumerator GetEnumerator() => new ReadOnlySpan<bool>(fields).GetEnumerator();

    public ReadOnlySpan<char> AsSpan()
    {
        // Since `bool` does not implement `ISpanFormattable` (see: https://github.com/dotnet/runtime/issues/67388),
        // we cant use `Helpers.AsSpan`, and are forced to wrap it in a struct that implements it.

        int tupleLength = Length;

        SFBool[] sfBools = new SFBool[tupleLength];
        for (int i = 0; i < tupleLength; i++)
        {
            sfBools[i] = new(this[i]);
        }

        return new SFBoolTuple(sfBools).AsSpan(Constants.MaxFieldCharLength_Bool);
    }

    readonly record struct SFBoolTuple(params SFBool[] Fields) : ISpaceTuple<SFBool>
    {
        public ref readonly SFBool this[int index] => ref Fields[index];
        public int Length => Fields.Length;

        public ReadOnlySpan<char> AsSpan() => ReadOnlySpan<char>.Empty;
        public ReadOnlySpan<SFBool>.Enumerator GetEnumerator() => new ReadOnlySpan<SFBool>(Fields).GetEnumerator();
    }

    readonly record struct SFBool(bool Value) : ISpanFormattable
    {
        public string ToString(string? format, IFormatProvider? formatProvider) => Value.ToString();

        public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
            => Value.TryFormat(destination, out charsWritten);
    }
}

public readonly record struct BoolTemplate : ISpaceTemplate<bool>
{
    private readonly bool?[] fields;

    public ref readonly bool? this[int index] => ref fields[index];
    public int Length => fields.Length;

    public BoolTemplate([AllowNull] params bool?[] fields)
        => this.fields = fields == null || fields.Length == 0 ? new bool?[1] { null } : fields;

    public bool Matches<TTuple>(TTuple tuple) where TTuple : ISpaceTuple<bool> 
        => TupleHelpers.Matches(this, tuple);

    ISpaceTuple<bool> ISpaceTemplate<bool>.Create(bool[] fields) => new BoolTuple(fields);

    public override string ToString() => TupleHelpers.ToString(fields);
    public ReadOnlySpan<bool?>.Enumerator GetEnumerator() => new ReadOnlySpan<bool?>(fields).GetEnumerator();
}
