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


    [Benchmark(Baseline = true)]
    public void Faster()
    {
        for (int i = 0; i < Iterations; i++)
        {
         
        }
    }

    [Benchmark]
    public void Solwer()
    {
        Span<char> span = stackalloc char[48];
        for (int i = 0; i < Iterations; i++)
        {
            
        }
    }
}
