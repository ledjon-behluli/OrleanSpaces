using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using OrleanSpaces.Tuples;
using OrleanSpaces.Tuples.Typed;

[ShortRunJob]
[MemoryDiagnoser]
[CategoriesColumn]
[Orderer(SummaryOrderPolicy.Declared)]
public class DateTimeTupleBenchmarks
{
    private const int iterations = 100_000;

    [Benchmark(Baseline = true)]
    public void DateTimeTuple()
    {
        DateTimeTuple tuple = new(new DateTime[] { DateTime.Now, DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), DateTime.Now.AddDays(3) });

        for (int i = 0; i < iterations; i++)
            tuple.Equals(tuple);
    }

    [Benchmark]
    public void DateTimeTupleAsNumerics()
    {
        DateTimeTuple_Optimized_V1 tuple = new(new DateTime[] { DateTime.Now, DateTime.Now.AddDays(1), DateTime.Now.AddDays(2), DateTime.Now.AddDays(3) });

        for (int i = 0; i < iterations; i++)
            tuple.Equals(tuple);
    }
}
