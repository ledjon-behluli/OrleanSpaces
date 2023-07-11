using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using OrleanSpaces.Tuples.Specialized;

//TODO: Delete me!
[ShortRunJob]
[MemoryDiagnoser]
[CategoriesColumn]
[Orderer(SummaryOrderPolicy.Declared)]
public class TestBench
{
    [Params(1_000, 10_000, 100_000)]
    public int Iterations { get; set; }

    private readonly IntTuple inttuple = new(1, 2, 3, 4, 5);

    [Benchmark]
    public void ToTemplate()
    {
      
    }

    [Benchmark]
    public void ToTemplateCopy()
    {
      
    }
}