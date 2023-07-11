﻿using Newtonsoft.Json;
using OrleanSpaces.Helpers;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples.Specialized;

[GenerateSerializer, Immutable]
public readonly record struct BoolTuple :
    IEquatable<BoolTuple>,
    ISpaceTuple<bool>,
    ISpaceFactory<bool, BoolTuple>,
    ISpaceConvertible<bool, BoolTemplate>
{
    [Id(0), JsonProperty] private readonly bool[] fields;
    [JsonIgnore] public int Length => fields?.Length ?? 0;

    public ref readonly bool this[int index] => ref fields[index];

    public BoolTuple() => fields = Array.Empty<bool>();
    public BoolTuple([AllowNull] params bool[] fields)
        => this.fields = fields is null ? Array.Empty<bool>() : fields;

    public BoolTemplate ToTemplate()
    {
        int length = Length;
        bool?[] fields = new bool?[length];

        for (int i = 0; i < length; i++)
        {
            fields[i] = this[i];
        }

        return new BoolTemplate(fields);
    }

    public bool Equals(BoolTuple other)
    {
        NumericMarshaller<bool, byte> marshaller = new(fields.AsSpan(), other.fields.AsSpan());
        return marshaller.TryParallelEquals(out bool result) ? result : this.SequentialEquals(other);
    }

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => SpaceHelpers.ToString(fields);

    static BoolTuple ISpaceFactory<bool, BoolTuple>.Create(bool[] fields) => new(fields);

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

public readonly record struct BoolTemplate : 
    IEquatable<BoolTemplate>, 
    ISpaceTemplate<bool>,
    ISpaceMatchable<bool, BoolTuple>
{
    private readonly bool?[] fields;

    public ref readonly bool? this[int index] => ref fields[index];
    public int Length => fields?.Length ?? 0;

    public BoolTemplate() => fields = Array.Empty<bool?>();
    public BoolTemplate([AllowNull] params bool?[] fields)
        => this.fields = fields is null ? Array.Empty<bool?>() : fields;

    public bool Matches(BoolTuple tuple) => this.Matches<bool, BoolTuple>(tuple);
    public bool Equals(BoolTemplate other) => this.SequentialEquals(other);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => SpaceHelpers.ToString(fields);

    public ReadOnlySpan<bool?>.Enumerator GetEnumerator() => new ReadOnlySpan<bool?>(fields).GetEnumerator();

}
