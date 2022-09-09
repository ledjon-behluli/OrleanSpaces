using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using OrleanSpaces.Primitives;

//BenchmarkRunner.Run<SpaceUnitBenchmarks>();
//BenchmarkRunner.Run<SpaceTupleBenchmarks>();
//BenchmarkRunner.Run<SpaceTemplateBenchmarks>();
//BenchmarkRunner.Run<TaskPartitionerBenchmarks>();
BenchmarkRunner.Run<SpaceTupleB>();

Console.ReadKey();


[MemoryDiagnoser]
[ShortRunJob]
public class SpaceTupleB
{
    private const int iterations = 100_000;

    private readonly static SpaceTuple tuple = 
        SpaceTuple.Create(
            (1, "a", 1.5f, 'b', 1.2m, 2, 0x00, -3, 2.4d, 1, "a", 1.5f, 'b', 1.2m, 2, 0x00, -3, 2.4d,
             1, "a", 1.5f, 'b', 1.2m, 2, 0x00, -3, 2.4d, 1, "a", 1.5f, 'b', 1.2m, 2, 0x00, -3, 2.4d,
             1, "a", 1.5f, 'b', 1.2m, 2, 0x00, -3, 2.4d, 1, "a", 1.5f, 'b', 1.2m, 2, 0x00, -3, 2.4d,
             1, "a", 1.5f, 'b', 1.2m, 2, 0x00, -3, 2.4d, 1, "a", 1.5f, 'b', 1.2m, 2, 0x00, -3, 2.4d,
             1, "a", 1.5f, 'b', 1.2m, 2, 0x00, -3, 2.4d, 1, "a", 1.5f, 'b', 1.2m, 2, 0x00, -3, 2.4d,
             1, "a", 1.5f, 'b', 1.2m, 2, 0x00, -3, 2.4d, 1, "a", 1.5f, 'b', 1.2m, 2, 0x00, -3, 2.4d,
             1, "a", 1.5f, 'b', 1.2m, 2, 0x00, -3, 2.4d, 1, "a", 1.5f, 'b', 1.2m, 2, 0x00, -3, 2.4d,
             1, "a", 1.5f, 'b', 1.2m, 2, 0x00, -3, 2.4d, 1, "a", 1.5f, 'b', 1.2m, 2, 0x00, -3, 2.4d));

    [Benchmark]
    public void Array()
    {
        int length = tuple.Length;
        object[] array = new object[length]; 

        for (int i = 0; i < length; i++)
            array[i] = tuple[i];
    }

    [Benchmark]
    public void Span()
    {
        int length = tuple.Length;
        object[] array = new object[tuple.Length];

        ReadOnlySpan<object> span = tuple.AsSpan();
        for (int i = 0; i < length; i++)
        {
            array[i] = span[i];
        }
    }
}