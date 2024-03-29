﻿using Newtonsoft.Json;
using OrleanSpaces.Helpers;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.Intrinsics;

namespace OrleanSpaces.Tuples.Specialized;

/// <summary>
/// Represents a tuple which has <see cref="decimal"/> field types only.
/// </summary>
[GenerateSerializer, Immutable]
public readonly record struct DecimalTuple :
    IEquatable<DecimalTuple>,
    IEqualityOperators<DecimalTuple, DecimalTuple, bool>,
    ISpaceTuple<decimal>,
    ISpaceFactory<decimal, DecimalTuple>,
    ISpaceConvertible<decimal, DecimalTemplate>
{
    [Id(0), JsonProperty] private readonly decimal[] fields;
    [JsonIgnore] public int Length => fields?.Length ?? 0;
    [JsonIgnore] public bool IsEmpty => Length == 0;

    public ref readonly decimal this[int index] => ref fields[index];

    /// <summary>
    /// Initializes an empty tuple.
    /// </summary>
    public DecimalTuple() : this(null) { }

    /// <summary>
    /// If <paramref name="fields"/> is <see langword="null"/>, initializes an empty tuple.
    /// </summary>
    /// <param name="fields">The elements of this tuple.</param>
    public DecimalTuple([AllowNull] params decimal[] fields)
        => this.fields = fields is null ? Array.Empty<decimal>() : fields;

    /// <summary>
    /// Returns a <see cref="DecimalTemplate"/> with the same fields as <see langword="this"/>.
    /// </summary>
    /// <remarks><i>If <see cref="Length"/> is 0, than default <see cref="DecimalTemplate"/> is created.</i></remarks>
    public DecimalTemplate ToTemplate()
    {
        int length = Length;
        decimal?[] fields = new decimal?[length];

        for (int i = 0; i < length; i++)
        {
            fields[i] = this[i];
        }

        return new DecimalTemplate(fields);
    }

    public bool Equals(DecimalTuple other)
    {
        if (Length != other.Length)
        {
            return false;
        }

        if (Length == 1)
        {
            // If 'this' and 'other' are of tupleLength = 1, then there is no benefit to perform vector equality (even if there is hardware supports) as the vectors will contain 4 int's.
            // Since this part of the code will only run if 256-bit vector operations are subject to hardware acceleration, it means that [4 / Vector<int>.Count] = [4 / 8] = [0].
            // This effectively means switching to sequential comparission between the 4 int's, so it makes sense to directly compare the 2 decimals.

            return this[0] == other[0];
        }

        // We check if 256-bit vector operations are subject to hardware acceleration because a decimal is converted to its equivalent binary representation via 4 int's.
        // If the hardware supported only 128-bit vector operations it would result in [Vector<int>.Count = 4], which means 4 int comparission in parallel.
        // While true that it would be a single processor instruction, so is the direct comparission between 2 decimals, thereby we go this route only if there is 256-bit vector support
        // as this allows equality checking between 4 decimals (2 from 'this' and 2 from 'other', as opposed to 1 from 'this' and 1 from 'other').

        if (!Vector256.IsHardwareAccelerated)
        {
            return this.SequentialEquals<decimal, DecimalTuple>(other);
        }

        return new Comparer(this, other).AllocateAndExecute<int, Comparer>(8 * Length); // 8 x Length, because each decimal will be decomposed into 4 ints, and we have 2 tuples to compare.
    }

    public override int GetHashCode() => fields?.GetHashCode() ?? 0;
    public override string ToString() => TupleHelpers.ToString(fields);

    static DecimalTuple ISpaceFactory<decimal, DecimalTuple>.Create(decimal[] fields) => new(fields);

    public ReadOnlySpan<char> AsSpan() => this.AsSpan<decimal, DecimalTuple>(Constants.MaxFieldCharLength_Decimal);
    public ReadOnlySpan<decimal>.Enumerator GetEnumerator() => 
        (fields is null ? ReadOnlySpan<decimal>.Empty : new ReadOnlySpan<decimal>(fields)).GetEnumerator();

    readonly struct Comparer : IBufferConsumer<int>
    {
        private readonly DecimalTuple left;
        private readonly DecimalTuple right;

        public Comparer(DecimalTuple left, DecimalTuple right)
        {
            this.left = left;
            this.right = right;
        }

        public bool Consume(ref Span<int> buffer)
        {
            int tupleLength = left.Length;
            int bufferHalfLength = buffer.Length / 2;

            Span<int> leftSpan = buffer[..bufferHalfLength];
            Span<int> rightSpan = buffer[bufferHalfLength..];

            for (int i = 0; i < tupleLength; i++)
            {
                decimal.GetBits(left[i], leftSpan.Slice(i * 4, 4));
                decimal.GetBits(right[i], rightSpan.Slice(i * 4, 4));
            }

            return leftSpan.ParallelEquals(rightSpan);
        }
    }
}

/// <summary>
/// Represents a template which has <see cref="decimal"/> field types only.
/// </summary>
public readonly record struct DecimalTemplate :
    IEquatable<DecimalTemplate>,
    IEqualityOperators<DecimalTemplate, DecimalTemplate, bool>,
    ISpaceTemplate<decimal>, 
    ISpaceMatchable<decimal, DecimalTuple>
{
    private readonly decimal?[] fields;

    public ref readonly decimal? this[int index] => ref fields[index];
    public int Length => fields?.Length ?? 0;

    /// <summary>
    /// Initializes a template with a single <see langword="null"/> field.
    /// </summary>
    public DecimalTemplate() : this(null) { }

    /// <summary>
    /// If <paramref name="fields"/> is <see langword="null"/>, initializes a template with a single <see langword="null"/> field.
    /// </summary>
    /// <param name="fields">The elements of this template.</param>
    public DecimalTemplate([AllowNull] params decimal?[] fields)
        => this.fields = fields is null || fields.Length == 0 ? new decimal?[1] { null } : fields;

    /// <summary>
    /// Determines whether <see langword="this"/> matches the specified <paramref name="tuple"/>.
    /// </summary>
    /// <param name="tuple">A tuple to be matched by <see langword="this"/>.</param>
    /// <returns><see langword="true"/>, if <see langword="this"/> and <paramref name="tuple"/> share the same number of fields, and all of them match on the index and value 
    /// (<i>except when any field of <see langword="this"/> is of type <see langword="null"/></i>); otherwise, <see langword="false"/>.</returns>
    public bool Matches(DecimalTuple tuple) => this.Matches<decimal, DecimalTuple, DecimalTemplate>(tuple);
    public bool Equals(DecimalTemplate other) => this.SequentialEquals<decimal, DecimalTemplate>(other);

    public override int GetHashCode() => fields?.GetHashCode() ?? 0;
    public override string ToString() => TemplateHelpers.ToString(fields);

    public ReadOnlySpan<decimal?>.Enumerator GetEnumerator() => 
        (fields is null ? ReadOnlySpan<decimal?>.Empty : new ReadOnlySpan<decimal?>(fields)).GetEnumerator();
}
