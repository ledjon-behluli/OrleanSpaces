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


    static readonly bool checkLengths = true;
    static readonly int[] array1 = new int[] { 1, 0, 1, 0 };
    static readonly int[] array2 = new int[] { 1, 0, 1, 0 };

    [Benchmark(Baseline = true)]
    public void A()
    {
        for (int i = 0; i < Iterations; i++)
        {
            _ = checkLengths && array1.Length != array2.Length;
        }
    }

    [Benchmark]
    public void B()
    {
        for (int i = 0; i < Iterations; i++)
        {
            _ = array1.Length != array2.Length;
        }
    }
}
