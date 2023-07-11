using Newtonsoft.Json;
using OrleanSpaces.Helpers;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Intrinsics;

namespace OrleanSpaces.Tuples.Specialized;

[GenerateSerializer, Immutable]
public readonly record struct DecimalTuple :
    IEquatable<DecimalTuple>,
    ISpaceTuple<decimal>,
    ISpaceFactory<decimal, DecimalTuple>,
    ISpaceConvertible<decimal, DecimalTemplate>
{
    [Id(0), JsonProperty] private readonly decimal[] fields;
    [JsonIgnore] public int Length => fields.Length;

    public ref readonly decimal this[int index] => ref fields[index];

    public DecimalTuple() => fields = Array.Empty<decimal>();
    public DecimalTuple([AllowNull] params decimal[] fields)
        => this.fields = fields is null ? Array.Empty<decimal>() : fields;

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
            return this.SequentialEquals(other);
        }

        return new Comparer(this, other).AllocateAndExecute(8 * Length); // 8 x Length, because each decimal will be decomposed into 4 ints, and we have 2 tuples to compare.
    }

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => SpaceHelpers.ToString(fields);

    static DecimalTuple ISpaceFactory<decimal, DecimalTuple>.Create(decimal[] fields) => new(fields);

    public ReadOnlySpan<char> AsSpan() => this.AsSpan(Constants.MaxFieldCharLength_Decimal);
    public ReadOnlySpan<decimal>.Enumerator GetEnumerator() => new ReadOnlySpan<decimal>(fields).GetEnumerator();

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

public readonly record struct DecimalTemplate :
    IEquatable<DecimalTemplate>,
    ISpaceTemplate<decimal>, 
    ISpaceMatchable<decimal, DecimalTuple>
{
    private readonly decimal?[] fields;

    public ref readonly decimal? this[int index] => ref fields[index];
    public int Length => fields.Length;

    public DecimalTemplate() => fields = Array.Empty<decimal?>();
    public DecimalTemplate([AllowNull] params decimal?[] fields)
        => this.fields = fields is null ? Array.Empty<decimal?>() : fields;

    public bool Matches(DecimalTuple tuple) => this.Matches<decimal, DecimalTuple>(tuple);
    public bool Equals(DecimalTemplate other) => this.SequentialEquals(other);

    public override int GetHashCode() => fields.GetHashCode();
    public override string ToString() => SpaceHelpers.ToString(fields);

    public ReadOnlySpan<decimal?>.Enumerator GetEnumerator() => new ReadOnlySpan<decimal?>(fields).GetEnumerator();
}
