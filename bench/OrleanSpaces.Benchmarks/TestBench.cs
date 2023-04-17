using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

[ShortRunJob]
[MemoryDiagnoser]
[CategoriesColumn]
[Orderer(SummaryOrderPolicy.Declared)]
public class TestBench
{
    [Params(100_000)]
    public int Iterations { get; set; }

    [Benchmark(Baseline = true)]
    public void A()
    {
       
    }

    [Benchmark]
    public void B()
    {
       
    }
}
