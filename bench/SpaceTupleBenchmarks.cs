using BenchmarkDotNet.Attributes;

[MemoryDiagnoser]
[ShortRunJob]
public class SpaceTupleBenchmarks
{
    [Benchmark]
    public void Test() { }
}
