using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using OrleanSpaces.Tuples.Typed;

[ShortRunJob]
[MemoryDiagnoser]
[CategoriesColumn]
[Orderer(SummaryOrderPolicy.Declared)]
public class TupleToStringBenchmarks
{
    [Params(1_000, 10_000, 100_000, 1_000_000)]
    public int Iterations { get; set; }

    private static readonly StringJoinTuple stringJoinTuple = new(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);
    private static readonly IntTuple intTuple = new(1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1);

    [BenchmarkCategory("Empty"), Benchmark]
    public void StringJoin_Empty()
    {
        StringJoinTuple tuple = new();

        for (int i = 0; i < Iterations; i++)
            _ = tuple.ToString();
    }

    [BenchmarkCategory("Empty"), Benchmark]
    public void Custom_Empty()
    {
        IntTuple tuple = new();

        for (int i = 0; i < Iterations; i++)
            _ = tuple.ToString();
    }

    [BenchmarkCategory("Single"), Benchmark]
    public void StringJoin_Single()
    {
        StringJoinTuple tuple = new();

        for (int i = 0; i < Iterations; i++)
            _ = tuple.ToString();
    }

    [BenchmarkCategory("Single"), Benchmark]
    public void Custom_Single()
    {
        IntTuple tuple = new();

        for (int i = 0; i < Iterations; i++)
            _ = tuple.ToString();
    }

    [BenchmarkCategory("XS"), Benchmark]
    public void StringJoin_XS()
    {
        StringJoinTuple tuple = new();

        for (int i = 0; i < Iterations; i++)
            _ = tuple.ToString();
    }

    [BenchmarkCategory("XS"), Benchmark]
    public void Custom_XS()
    {
        IntTuple tuple = new();

        for (int i = 0; i < Iterations; i++)
            _ = tuple.ToString();
    }

    //TODO: Continue with rest, and run benchmarks

    public readonly struct StringJoinTuple : INumericSpaceTuple<int, StringJoinTuple>
    {
        private readonly int[] fields;

        public int this[int index] => fields[index];
        public int Length => fields.Length;

        public StringJoinTuple() : this(Array.Empty<int>()) { }
        public StringJoinTuple(params int[] fields) => this.fields = fields;

        public static bool operator ==(StringJoinTuple left, StringJoinTuple right) => left.Equals(right);
        public static bool operator !=(StringJoinTuple left, StringJoinTuple right) => !(left == right);

        public ReadOnlySpan<int> AsSpan() => fields.AsSpan();

        public override bool Equals(object obj) => obj is StringJoinTuple tuple && Equals(tuple);
        public bool Equals(StringJoinTuple other) => true;

        public int CompareTo(StringJoinTuple other) => Length.CompareTo(other.Length);

        public override int GetHashCode() => fields.GetHashCode();

        public override string ToString() => $"({string.Join(", ", fields)})";
    }
}
