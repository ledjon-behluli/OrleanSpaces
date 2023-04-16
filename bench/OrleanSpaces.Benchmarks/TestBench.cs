using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using OrleanSpaces.Tuples.Typed;

[ShortRunJob]
[MemoryDiagnoser]
[CategoriesColumn]
[Orderer(SummaryOrderPolicy.Declared)]
public class TestBench
{
    private const int iterations = 100_000;

    [Params(100, 1_000, 10_000, 100_000, 1_000_000)]
    public int Iterations { get; set; }

    [Benchmark(Baseline = true)]
    public void NonOpt()
    {
        for (int i = 0; i < Iterations; i++)
            _ = new DateTimeTuple(new DateTime[] { 
                DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now,
                DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now});
    }

    [Benchmark]
    public void Opt()
    {
        for (int i = 0; i < Iterations; i++)
            _ = new DateTimeTuple_Optimized_V1(new DateTime[] {
                DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now,
                DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now});
    }

    [Benchmark]
    public void OptV2()
    {
        for (int i = 0; i < Iterations; i++)
            _ = new DateTimeTuple_Optimized_V2(new DateTime[] {
                DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now,
                DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now});
    }

    [Benchmark]
    public void OptV3()
    {
        for (int i = 0; i < Iterations; i++)
            _ = new DateTimeTuple_Optimized_V3(new DateTime[] {
                DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now,
                DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now});
    }

    //[Benchmark(Baseline = true)]
    //public void NonOpt()
    //{
    //    DateTimeTuple tuple = new(new DateTime[] { DateTime.Now, DateTime.Now.AddDays(1), DateTime.Now, DateTime.Now });

    //    for (int i = 0; i < iterations; i++)
    //        tuple.Equals(tuple);
    //}

    //[Benchmark]
    //public void Opt()
    //{
    //    DateTimeTuple_Optimized_V1 tuple = new(new DateTime[] { DateTime.Now, DateTime.Now.AddDays(1), DateTime.Now, DateTime.Now });

    //    for (int i = 0; i < iterations; i++)
    //        tuple.Equals(tuple);
    //}
}
