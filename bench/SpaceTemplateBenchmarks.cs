using BenchmarkDotNet.Attributes;
using OrleanSpaces.Primitives;

[MemoryDiagnoser]
[ShortRunJob]
[CategoriesColumn]
public class SpaceTemplateBenchmarks
{
    private const int iterations = 100_000;

    private readonly static SpaceTemplate baseTemplate = SpaceTemplate.Create(1);
    private readonly static SpaceTemplate shortTemplate = SpaceTemplate.Create((1, "a"));
    private readonly static SpaceTemplate mediumTemplate = SpaceTemplate.Create((1, "a", 1.5f, 'b', 1.2m, 2, 0x00, -3, 2.4d));
    private readonly static SpaceTemplate longTemplate = SpaceTemplate.Create((1, "a", 1.5f, 'b', 1.2m, 2, 0x00, -3, 2.4d, 1, "a", 1.5f, 'b', 1.2m, 2, 0x00, -3, 2.4d));

    [BenchmarkCategory("Short Template"), Benchmark]
    public void ShortSameLength()
    {
        for (int i = 0; i < iterations; i++)
            shortTemplate.Equals(shortTemplate);
    }

    [BenchmarkCategory("Short Template"), Benchmark]
    public void ShortDiffLengths()
    {
        for (int i = 0; i < iterations; i++)
            shortTemplate.Equals(baseTemplate);
    }

    [BenchmarkCategory("Medium Template"), Benchmark]
    public void MediumSameLength()
    {
        for (int i = 0; i < iterations; i++)
            mediumTemplate.Equals(mediumTemplate);
    }

    [BenchmarkCategory("Medium Template"), Benchmark]
    public void MediumDiffLengths()
    {
        for (int i = 0; i < iterations; i++)
            mediumTemplate.Equals(baseTemplate);
    }

    [BenchmarkCategory("Long Template"), Benchmark]
    public void LongSameLength()
    {
        for (int i = 0; i < iterations; i++)
            longTemplate.Equals(longTemplate);
    }

    [BenchmarkCategory("Long Template"), Benchmark]
    public void LongDiffLengths()
    {
        for (int i = 0; i < iterations; i++)
            longTemplate.Equals(baseTemplate);
    }
}
