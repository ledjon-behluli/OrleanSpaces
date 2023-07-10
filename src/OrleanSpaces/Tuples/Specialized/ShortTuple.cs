﻿using Newtonsoft.Json;
using OrleanSpaces.Helpers;
using System.Diagnostics.CodeAnalysis;

namespace OrleanSpaces.Tuples.Specialized;

[GenerateSerializer, Immutable]
public readonly struct ShortTuple : INumericTuple<short>, IEquatable<ShortTuple>
{
    [Id(0), JsonProperty] private readonly short[] fields;
    [JsonIgnore] public int Length => fields.Length;

    public ref readonly short this[int index] => ref fields[index];

    Span<short> INumericTuple<short>.Fields => fields.AsSpan();

    public ShortTuple() => fields = Array.Empty<short>();
    public ShortTuple([AllowNull] params short[] fields)
        => this.fields = fields is null ? Array.Empty<short>() : fields;

    public static bool operator ==(ShortTuple left, ShortTuple right) => left.Equals(right);
    public static bool operator !=(ShortTuple left, ShortTuple right) => !(left == right);

    public static explicit operator ShortTemplate(ShortTuple tuple)
    {
        int length = tuple.Length;
        short?[] fields = new short?[length];

        for (int i = 0; i < length; i++)
        {
            fields[i] = tuple[i];
        }

        return new ShortTemplate(fields);
    }

    public override bool Equals(object? obj) => obj is ShortTuple tuple && Equals(tuple);
    public bool Equals(ShortTuple other)
        => this.TryParallelEquals(other, out bool result) ? result : this.SequentialEquals(other);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => TupleHelpers.ToString(fields);

    ISpaceTemplate<short> ISpaceTuple<short>.ToTemplate() => (ShortTemplate)this;
    static ISpaceTuple<short> ISpaceTuple<short>.Create(short[] fields) => new ShortTuple(fields);

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(Constants.MaxFieldCharLength_Short);
    public ReadOnlySpan<short>.Enumerator GetEnumerator() => new ReadOnlySpan<short>(fields).GetEnumerator();
}

public readonly record struct ShortTemplate : ISpaceTemplate<short>
{
    private readonly short?[] fields;

    public ref readonly short? this[int index] => ref fields[index];
    public int Length => fields.Length;

    public ShortTemplate() => fields = Array.Empty<short?>();
    public ShortTemplate([AllowNull] params short?[] fields)
        => this.fields = fields is null ? Array.Empty<short?>() : fields;

    public bool Matches<TTuple>(TTuple tuple) where TTuple : ISpaceTuple<short>
        => TupleHelpers.Matches<short, ShortTuple>(this, tuple);

    public override string ToString() => TupleHelpers.ToString(fields);
    public ReadOnlySpan<short?>.Enumerator GetEnumerator() => new ReadOnlySpan<short?>(fields).GetEnumerator();
}