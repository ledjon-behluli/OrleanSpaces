using BenchmarkDotNet.Attributes;
using OrleanSpaces.Primitives;

[MemoryDiagnoser]
[ShortRunJob]
public class TestRun
{
    [Params(1, 10, 100)]
    public int Iterations { get; set; }

    [Benchmark]
    public void Normal()
    {
        for (int i = 0; i < Iterations; i++)
        {
            SpaceTupleAlloc.Create((1, "a", 1.5f, 'b', 1.2m, 2, 0x00, -3, 2.4d));
        }
    }

    [Benchmark]
    public void Pool()
    {
        for (int i = 0; i < Iterations; i++)
        {
            SpaceTupleAlloc.CreatePool((1, "a", 1.5f, 'b', 1.2m, 2, 0x00, -3, 2.4d));
        }
    }
}