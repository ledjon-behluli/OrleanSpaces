using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using OrleanSpaces.Tuples;

[MemoryDiagnoser]
[CategoriesColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class SpaceTemplateBenchmarks
{
    private const int iterations = 100_000;

    #region Instantiation

    [BenchmarkCategory("Instantiation"), Benchmark]
    public void InstantiateByCtor()
    {
        for (int i = 0; i < iterations; i++)
        {
            _ = new SpaceTemplate(null);
            _ = new SpaceTemplate(null, null);
            _ = new SpaceTemplate(null, null, null);
            _ = new SpaceTemplate(null, null, null, null);
            _ = new SpaceTemplate(null, null, null, null, null);
            _ = new SpaceTemplate(null, null, null, null, null, null);
            _ = new SpaceTemplate(null, null, null, null, null, null, null);
            _ = new SpaceTemplate(null, null, null, null, null, null, null, null);
        }
    }

    [BenchmarkCategory("Instantiation"), Benchmark]
    public void InstantiateByRefReturn()
    {
        for (int i = 0; i < iterations; i++)
        {
            _ = SpaceTemplateCache.Tuple_1;
            _ = SpaceTemplateCache.Tuple_2;
            _ = SpaceTemplateCache.Tuple_3;
            _ = SpaceTemplateCache.Tuple_4;
            _ = SpaceTemplateCache.Tuple_5;
            _ = SpaceTemplateCache.Tuple_6;
            _ = SpaceTemplateCache.Tuple_7;
            _ = SpaceTemplateCache.Tuple_8;
        }
    }

    #endregion

    #region Equality

    private readonly static SpaceTemplate baseTemplate = new(1);
    private readonly static SpaceTemplate shortTemplate = new(1, "a");
    private readonly static SpaceTemplate mediumTemplate = new(1, "a", 1.5f, 'b', 1.2m, 2, 0x00, -3, 2.4d);
    private readonly static SpaceTemplate longTemplate = new(1, "a", 1.5f, 'b', 1.2m, 2, 0x00, -3, 2.4d, 1, "a", 1.5f, 'b', 1.2m, 2, 0x00, -3, 2.4d);

    [BenchmarkCategory("Equality", "Short"), Benchmark]
    public void ShortSameLength()
    {
        for (int i = 0; i < iterations; i++)
            shortTemplate.Equals(shortTemplate);
    }

    [BenchmarkCategory("Equality", "Short"), Benchmark]
    public void ShortDiffLengths()
    {
        for (int i = 0; i < iterations; i++)
            shortTemplate.Equals(baseTemplate);
    }

    [BenchmarkCategory("Equality", "Medium"), Benchmark]
    public void MediumSameLength()
    {
        for (int i = 0; i < iterations; i++)
            mediumTemplate.Equals(mediumTemplate);
    }

    [BenchmarkCategory("Equality", "Medium"), Benchmark]
    public void MediumDiffLengths()
    {
        for (int i = 0; i < iterations; i++)
            mediumTemplate.Equals(baseTemplate);
    }

    [BenchmarkCategory("Equality", "Long"), Benchmark]
    public void LongSameLength()
    {
        for (int i = 0; i < iterations; i++)
            longTemplate.Equals(longTemplate);
    }

    [BenchmarkCategory("Equality", "Long"), Benchmark]
    public void LongDiffLengths()
    {
        for (int i = 0; i < iterations; i++)
            longTemplate.Equals(baseTemplate);
    }

    #endregion

    #region Matching

    private readonly static SpaceTuple tuple = new(1, "a", 1.5f);
    private readonly static SpaceTemplate template1 = new(1, "a", 1.5f);
    private readonly static SpaceTemplate template2 = new(1, null, 1.5f);
    private readonly static SpaceTemplate template3 = new(1, "a", 2.5f);
    private readonly static SpaceTemplate template4 = new(1, "a");

    [BenchmarkCategory("Matching"), Benchmark]
    public void FullMatch()
    {
        for (int i = 0; i < iterations; i++)
            template1.Matches(tuple);
    }

    [BenchmarkCategory("Matching"), Benchmark]
    public void PartialMatch()
    {
        for (int i = 0; i < iterations; i++)
            template2.Matches(tuple);
    }

    [BenchmarkCategory("Matching"), Benchmark]
    public void NoMatchSameLengths()
    {
        for (int i = 0; i < iterations; i++)
            template3.Matches(tuple);
    }

    [BenchmarkCategory("Matching"), Benchmark]
    public void NoMatchDiffLengths()
    {
        for (int i = 0; i < iterations; i++)
            template4.Matches(tuple);
    }

    #endregion
}

public readonly struct SpaceTemplateCache
{
    private static readonly SpaceTemplate tuple_1 = new(null);
    private static readonly SpaceTemplate tuple_2 = new(null, null);
    private static readonly SpaceTemplate tuple_3 = new(null, null, null);
    private static readonly SpaceTemplate tuple_4 = new(null, null, null, null);
    private static readonly SpaceTemplate tuple_5 = new(null, null, null, null, null);
    private static readonly SpaceTemplate tuple_6 = new(null, null, null, null, null, null);
    private static readonly SpaceTemplate tuple_7 = new(null, null, null, null, null, null, null);
    private static readonly SpaceTemplate tuple_8 = new(null, null, null, null, null, null, null, null);

    public static ref readonly SpaceTemplate Tuple_1 => ref tuple_1;
    public static ref readonly SpaceTemplate Tuple_2 => ref tuple_2;
    public static ref readonly SpaceTemplate Tuple_3 => ref tuple_3;
    public static ref readonly SpaceTemplate Tuple_4 => ref tuple_4;
    public static ref readonly SpaceTemplate Tuple_5 => ref tuple_5;
    public static ref readonly SpaceTemplate Tuple_6 => ref tuple_6;
    public static ref readonly SpaceTemplate Tuple_7 => ref tuple_7;
    public static ref readonly SpaceTemplate Tuple_8 => ref tuple_8;
}