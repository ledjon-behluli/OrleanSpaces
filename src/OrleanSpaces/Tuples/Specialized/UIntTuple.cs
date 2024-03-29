﻿using Newtonsoft.Json;
using OrleanSpaces.Helpers;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace OrleanSpaces.Tuples.Specialized;

/// <summary>
/// Represents a tuple which has <see cref="uint"/> field types only.
/// </summary>
[GenerateSerializer, Immutable]
public readonly record struct UIntTuple :
    IEquatable<UIntTuple>,
    IEqualityOperators<UIntTuple, UIntTuple, bool>,
    INumericTuple<uint>, 
    ISpaceFactory<uint, UIntTuple>,
    ISpaceConvertible<uint, UIntTemplate>
{
    [Id(0), JsonProperty] private readonly uint[] fields;
    [JsonIgnore] public int Length => fields?.Length ?? 0;
    [JsonIgnore] public bool IsEmpty => Length == 0;

    public ref readonly uint this[int index] => ref fields[index];

    Span<uint> INumericTuple<uint>.Fields => fields.AsSpan();

    /// <summary>
    /// Initializes an empty tuple.
    /// </summary>
    public UIntTuple() : this(null) { }

    /// <summary>
    /// If <paramref name="fields"/> is <see langword="null"/>, initializes an empty tuple.
    /// </summary>
    /// <param name="fields">The elements of this tuple.</param>
    public UIntTuple([AllowNull] params uint[] fields)
        => this.fields = fields is null ? Array.Empty<uint>() : fields;

    /// <summary>
    /// Returns a <see cref="UIntTemplate"/> with the same fields as <see langword="this"/>.
    /// </summary>
    /// <remarks><i>If <see cref="Length"/> is 0, than default <see cref="UIntTemplate"/> is created.</i></remarks>
    public UIntTemplate ToTemplate()
    {
        int length = Length;
        uint?[] fields = new uint?[length];

        for (int i = 0; i < length; i++)
        {
            fields[i] = this[i];
        }

        return new UIntTemplate(fields);
    }

    public bool Equals(UIntTuple other)
        => this.TryParallelEquals<uint, UIntTuple>(other, out bool result) ?
               result : this.SequentialEquals<uint, UIntTuple>(other);

    public override int GetHashCode() => fields?.GetHashCode() ?? 0;
    public override string ToString() => TupleHelpers.ToString(fields);

    static UIntTuple ISpaceFactory<uint, UIntTuple>.Create(uint[] fields) => new(fields);

    public ReadOnlySpan<char> AsSpan() => this.AsSpan<uint, UIntTuple>(Constants.MaxFieldCharLength_UInt);
    public ReadOnlySpan<uint>.Enumerator GetEnumerator() => 
        (fields is null ? ReadOnlySpan<uint>.Empty : new ReadOnlySpan<uint>(fields)).GetEnumerator();
}

/// <summary>
/// Represents a template which has <see cref="uint"/> field types only.
/// </summary>
public readonly record struct UIntTemplate :
    IEquatable<UIntTemplate>,
    IEqualityOperators<UIntTemplate, UIntTemplate, bool>,
    ISpaceTemplate<uint>, 
    ISpaceMatchable<uint, UIntTuple>
{
    private readonly uint?[] fields;

    public ref readonly uint? this[int index] => ref fields[index];
    public int Length => fields?.Length ?? 0;

    /// <summary>
    /// Initializes a template with a single <see langword="null"/> field.
    /// </summary>
    public UIntTemplate() : this(null) { }

    /// <summary>
    /// If <paramref name="fields"/> is <see langword="null"/>, initializes a template with a single <see langword="null"/> field.
    /// </summary>
    /// <param name="fields">The elements of this template.</param>
    public UIntTemplate([AllowNull] params uint?[] fields)
        => this.fields = fields is null || fields.Length == 0 ? new uint?[1] { null } : fields;

    /// <summary>
    /// Determines whether <see langword="this"/> matches the specified <paramref name="tuple"/>.
    /// </summary>
    /// <param name="tuple">A tuple to be matched by <see langword="this"/>.</param>
    /// <returns><see langword="true"/>, if <see langword="this"/> and <paramref name="tuple"/> share the same number of fields, and all of them match on the index and value 
    /// (<i>except when any field of <see langword="this"/> is of type <see langword="null"/></i>); otherwise, <see langword="false"/>.</returns>
    public bool Matches(UIntTuple tuple) => this.Matches<uint, UIntTuple, UIntTemplate>(tuple);
    public bool Equals(UIntTemplate other) => this.SequentialEquals<uint, UIntTemplate>(other);

    public override int GetHashCode() => fields?.GetHashCode() ?? 0;
    public override string ToString() => TemplateHelpers.ToString(fields);

    public ReadOnlySpan<uint?>.Enumerator GetEnumerator() => 
        (fields is null ? ReadOnlySpan<uint?>.Empty : new ReadOnlySpan<uint?>(fields)).GetEnumerator();
}