using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using OrleanSpaces.Tuples;
using OrleanSpaces.Tuples.Typed;

[ShortRunJob]
[MemoryDiagnoser]
[CategoriesColumn]
[Orderer(SummaryOrderPolicy.Declared)]
public class TestBench
{
    [Params(1_000)]//, 10_000, 100_000, 1_000_000)]
    public int Iterations { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        //array = new[] { 1, 1, 1, 1 };
        intTuple = new(1, 1, 1, 1);
    }

    //int[] array = new int[4];
    IntTuple intTuple;

    [Benchmark(Baseline = true)]
    public void Faster()
    {
        Span<char> span = stackalloc char[48];

        for (int i = 0; i < Iterations; i++)
        {
            intTuple.TryFormat(span, out _);
        }
    }

    //[Benchmark]
    //public void Solwer()
    //{
    //    for (int i = 0; i < Iterations; i++)
    //    {
         
    //    }
    //}
}
