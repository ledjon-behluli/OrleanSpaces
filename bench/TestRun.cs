using BenchmarkDotNet.Attributes;
using OrleanSpaces.Primitives;
using System.Runtime.CompilerServices;

[MemoryDiagnoser]
[ShortRunJob]
[CategoriesColumn]
public class TestRun
{
    //[Params(1)]
    public int Iterations { get; set; } = 1;

    //[Benchmark]
    //public void Normal()
    //{
    //    for (int i = 0; i < Iterations; i++)
    //    {
    //        SpaceTupleAlloc.Create((1, "a", 1.5f, 'b', 1.2m, 2, 0x00, -3, 2.4d));
    //    }
    //}

    //[Benchmark]
    //public void Pool()
    //{
    //    for (int i = 0; i < Iterations; i++)
    //    {
    //        SpaceTupleAlloc.CreatePool((1, "a", 1.5f, 'b', 1.2m, 2, 0x00, -3, 2.4d));
    //    }
    //}

    [BenchmarkCategory("SpaceTuple"), Benchmark]
    public void SpaceTuple_Normal_Int()
    {
        for (int i = 0; i < Iterations; i++)
        {
            SpaceTuple.Create(1);
        }
    }

    [BenchmarkCategory("SpaceTuple"), Benchmark]
    public void SpaceTuple_Normal_String()
    {
        for (int i = 0; i < Iterations; i++)
        {
            SpaceTuple.Create("a");
        }
    }

    [BenchmarkCategory("SpaceTuple"), Benchmark]
    public void SpaceTuple_Tuple()
    {
        for (int i = 0; i < Iterations; i++)
        {
            SpaceTuple.Create((1, "a", 1.5f, 'b', 1.2m, 2, 0x00, -3, 2.4d));
        }
    }

    /////////////////////////////////////////////////////

    [BenchmarkCategory("SpaceTupleAlloc"), Benchmark]
    public void SpaceTuple_Alloc_Int()
    {
        for (int i = 0; i < Iterations; i++)
        {
            SpaceTupleAlloc.Create(1);
        }
    }

    [BenchmarkCategory("SpaceTupleAlloc"), Benchmark]
    public void SpaceTuple_Alloc_String()
    {
        for (int i = 0; i < Iterations; i++)
        {
            SpaceTupleAlloc.Create("a");
        }
    }

    [BenchmarkCategory("SpaceTupleAlloc"), Benchmark]
    public void SpaceTuple_Alloc_Tuple()
    {
        for (int i = 0; i < Iterations; i++)
        {
            SpaceTupleAlloc.Create((1, "a", 1.5f, 'b', 1.2m, 2, 0x00, -3, 2.4d));
        }
    }
}