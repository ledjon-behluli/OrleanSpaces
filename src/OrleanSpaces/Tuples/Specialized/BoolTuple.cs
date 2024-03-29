﻿using Newtonsoft.Json;
using OrleanSpaces.Helpers;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace OrleanSpaces.Tuples.Specialized;

/// <summary>
/// Represents a tuple which has <see cref="bool"/> field types only.
/// </summary>
[GenerateSerializer, Immutable]
public readonly record struct BoolTuple :
    IEquatable<BoolTuple>,
    IEqualityOperators<BoolTuple, BoolTuple, bool>,
    ISpaceTuple<bool>,
    ISpaceFactory<bool, BoolTuple>,
    ISpaceConvertible<bool, BoolTemplate>
{
    [Id(0), JsonProperty] private readonly bool[] fields;
    [JsonIgnore] public int Length => fields?.Length ?? 0;
    [JsonIgnore] public bool IsEmpty => Length == 0;

    public ref readonly bool this[int index] => ref fields[index];

    /// <summary>
    /// Initializes an empty tuple.
    /// </summary>
    public BoolTuple() : this(null) { }

    /// <summary>
    /// If <paramref name="fields"/> is <see langword="null"/>, initializes an empty tuple.
    /// </summary>
    /// <param name="fields">The elements of this tuple.</param>
    public BoolTuple([AllowNull] params bool[] fields)
        => this.fields = fields is null ? Array.Empty<bool>() : fields;

    /// <summary>
    /// Returns a <see cref="BoolTemplate"/> with the same fields as <see langword="this"/>.
    /// </summary>
    /// <remarks><i>If <see cref="Length"/> is 0, than default <see cref="BoolTemplate"/> is created.</i></remarks>
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
        return marshaller.TryParallelEquals(out bool result) ? result : this.SequentialEquals<bool, BoolTuple>(other);
    }

    public override int GetHashCode() => fields?.GetHashCode() ?? 0;
    public override string ToString() => TupleHelpers.ToString(fields);

    static BoolTuple ISpaceFactory<bool, BoolTuple>.Create(bool[] fields) => new(fields);

    public ReadOnlySpan<bool>.Enumerator GetEnumerator() => 
        (fields is null ? ReadOnlySpan<bool>.Empty : new ReadOnlySpan<bool>(fields)).GetEnumerator();

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

        return new SFBoolTuple(sfBools).AsSpan<SFBool, SFBoolTuple>(Constants.MaxFieldCharLength_Bool);
    }

    readonly record struct SFBoolTuple(params SFBool[] Fields) : ISpaceTuple<SFBool>
    {
        public ref readonly SFBool this[int index] => ref Fields[index];
        public int Length => Fields.Length;
        public bool IsEmpty => Length == 0;

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

/// <summary>
/// Represents a template which has <see cref="bool"/> field types only.
/// </summary>
public readonly record struct BoolTemplate : 
    IEquatable<BoolTemplate>,
    IEqualityOperators<BoolTemplate, BoolTemplate, bool>,
    ISpaceTemplate<bool>,
    ISpaceMatchable<bool, BoolTuple>
{
    private readonly bool?[] fields;

    public ref readonly bool? this[int index] => ref fields[index];
    public int Length => fields?.Length ?? 0;

    /// <summary>
    /// Initializes a template with a single <see langword="null"/> field.
    /// </summary>
    public BoolTemplate() : this(null) { }

    /// <summary>
    /// If <paramref name="fields"/> is <see langword="null"/>, initializes a template with a single <see langword="null"/> field.
    /// </summary>
    /// <param name="fields">The elements of this template.</param>
    public BoolTemplate([AllowNull] params bool?[] fields)
        => this.fields = fields is null || fields.Length == 0 ? new bool?[] { null } : fields;

    /// <summary>
    /// Determines whether <see langword="this"/> matches the specified <paramref name="tuple"/>.
    /// </summary>
    /// <param name="tuple">A tuple to be matched by <see langword="this"/>.</param>
    /// <returns><see langword="true"/>, if <see langword="this"/> and <paramref name="tuple"/> share the same number of fields, and all of them match on the index and value 
    /// (<i>except when any field of <see langword="this"/> is of type <see langword="null"/></i>); otherwise, <see langword="false"/>.</returns>
    public bool Matches(BoolTuple tuple) => this.Matches<bool, BoolTuple, BoolTemplate>(tuple);
    public bool Equals(BoolTemplate other) => this.SequentialEquals<bool, BoolTemplate>(other);

    public override int GetHashCode() => fields?.GetHashCode() ?? 0;
    public override string ToString() => TemplateHelpers.ToString(fields);

    public ReadOnlySpan<bool?>.Enumerator GetEnumerator() => 
        (fields is null ? ReadOnlySpan<bool?>.Empty : new ReadOnlySpan<bool?>(fields)).GetEnumerator();
}
