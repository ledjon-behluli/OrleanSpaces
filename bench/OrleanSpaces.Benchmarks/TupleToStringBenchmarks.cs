using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using OrleanSpaces.Tuples;
using OrleanSpaces.Tuples.Typed;

[ShortRunJob]
[MemoryDiagnoser]
[CategoriesColumn]
[Orderer(SummaryOrderPolicy.Declared)]
public class TupleToStringBenchmarks
{
    private const int iterations = 100_000;

    [BenchmarkCategory("Empty"), Benchmark]
    public void StringJoin_Empty()
    {
        StringJoinIntTuple tuple = new();

        for (int i = 0; i < iterations; i++)
            _ = tuple.ToString();
    }

    [BenchmarkCategory("Empty"), Benchmark]
    public void Custom_Empty()
    {
        IntTuple tuple = new();

        for (int i = 0; i < iterations; i++)
            _ = tuple.ToString();
    }

    [BenchmarkCategory("Single"), Benchmark]
    public void StringJoin_Single()
    {
        StringJoinIntTuple tuple = new(1);

        for (int i = 0; i < iterations; i++)
            _ = tuple.ToString();
    }

    [BenchmarkCategory("Single"), Benchmark]
    public void Custom_Single()
    {
        IntTuple tuple = new(1);

        for (int i = 0; i < iterations; i++)
            _ = tuple.ToString();
    }

    [BenchmarkCategory("XS"), Benchmark]
    public void StringJoin_XS()
    {
        StringJoinIntTuple tuple = new(1, 1);

        for (int i = 0; i < iterations; i++)
            _ = tuple.ToString();
    }

    [BenchmarkCategory("XS"), Benchmark]
    public void Custom_XS()
    {
        IntTuple tuple = new(1, 1);

        for (int i = 0; i < iterations; i++)
            _ = tuple.ToString();
    }

    [BenchmarkCategory("S"), Benchmark]
    public void StringJoin_S()
    {
        StringJoinIntTuple tuple = new(1, 1, 1, 1);

        for (int i = 0; i < iterations; i++)
            _ = tuple.ToString();
    }

    [BenchmarkCategory("S"), Benchmark]
    public void Custom_S()
    {
        IntTuple tuple = new(1, 1, 1, 1);

        for (int i = 0; i < iterations; i++)
            _ = tuple.ToString();
    }

    [BenchmarkCategory("M"), Benchmark]
    public void StringJoin_M()
    {
        StringJoinIntTuple tuple = new(1, 1, 1, 1, 1, 1, 1, 1);

        for (int i = 0; i < iterations; i++)
            _ = tuple.ToString();
    }

    [BenchmarkCategory("M"), Benchmark]
    public void Custom_M()
    {
        IntTuple tuple = new(1, 1, 1, 1, 1, 1, 1, 1);

        for (int i = 0; i < iterations; i++)
            _ = tuple.ToString();
    }

    [BenchmarkCategory("L"), Benchmark]
    public void StringJoin_L()
    {
        StringJoinIntTuple tuple = new(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);

        for (int i = 0; i < iterations; i++)
            _ = tuple.ToString();
    }

    [BenchmarkCategory("L"), Benchmark]
    public void Custom_L()
    {
        IntTuple tuple = new(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);

        for (int i = 0; i < iterations; i++)
            _ = tuple.ToString();
    }

    [BenchmarkCategory("XL"), Benchmark]
    public void StringJoin_XL()
    {
        StringJoinIntTuple tuple = new(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);

        for (int i = 0; i < iterations; i++)
            _ = tuple.ToString();
    }

    [BenchmarkCategory("XL"), Benchmark]
    public void Custom_XL()
    {
        IntTuple tuple = new(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);

        for (int i = 0; i < iterations; i++)
            _ = tuple.ToString();
    }

    [BenchmarkCategory("XXL"), Benchmark]
    public void StringJoin_XXL()
    {
        StringJoinIntTuple tuple = new(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);

        for (int i = 0; i < iterations; i++)
            _ = tuple.ToString();
    }

    [BenchmarkCategory("XXL"), Benchmark]
    public void Custom_XXL()
    {
        IntTuple tuple = new(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);

        for (int i = 0; i < iterations; i++)
            _ = tuple.ToString();
    }

    public readonly struct StringJoinIntTuple : INumericSpaceTuple<int, StringJoinIntTuple>
    {
        private readonly int[] fields;

        public int this[int index] => fields[index];
        public int Length => fields.Length;

        Span<int> INumericSpaceTuple<int, StringJoinIntTuple>.Fields => fields.AsSpan();

        public StringJoinIntTuple() : this(Array.Empty<int>()) { }
        public StringJoinIntTuple(params int[] fields) => this.fields = fields;

        public static bool operator ==(StringJoinIntTuple left, StringJoinIntTuple right) => left.Equals(right);
        public static bool operator !=(StringJoinIntTuple left, StringJoinIntTuple right) => !(left == right);

        public override bool Equals(object obj) => obj is StringJoinIntTuple tuple && Equals(tuple);
        public bool Equals(StringJoinIntTuple other) => true;

        public int CompareTo(StringJoinIntTuple other) => Length.CompareTo(other.Length);

        public override int GetHashCode() => fields.GetHashCode();

        public override string ToString() => $"({string.Join(", ", fields)})";
    }
}
