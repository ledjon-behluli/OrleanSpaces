using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using OrleanSpaces.Tuples;

[ShortRunJob]
[MemoryDiagnoser]
[CategoriesColumn]
[Orderer(SummaryOrderPolicy.Declared)]
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
            _ = SpaceTemplateCache.Template_1;
            _ = SpaceTemplateCache.Template_2;
            _ = SpaceTemplateCache.Template_3;
            _ = SpaceTemplateCache.Template_4;
            _ = SpaceTemplateCache.Template_5;
            _ = SpaceTemplateCache.Template_6;
            _ = SpaceTemplateCache.Template_7;
            _ = SpaceTemplateCache.Template_8;
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
    private static readonly SpaceTemplate template_1 = new(null);
    private static readonly SpaceTemplate template_2 = new(null, null);
    private static readonly SpaceTemplate template_3 = new(null, null, null);
    private static readonly SpaceTemplate template_4 = new(null, null, null, null);
    private static readonly SpaceTemplate template_5 = new(null, null, null, null, null);
    private static readonly SpaceTemplate template_6 = new(null, null, null, null, null, null);
    private static readonly SpaceTemplate template_7 = new(null, null, null, null, null, null, null);
    private static readonly SpaceTemplate template_8 = new(null, null, null, null, null, null, null, null);

    public static ref readonly SpaceTemplate Template_1 => ref template_1;
    public static ref readonly SpaceTemplate Template_2 => ref template_2;
    public static ref readonly SpaceTemplate Template_3 => ref template_3;
    public static ref readonly SpaceTemplate Template_4 => ref template_4;
    public static ref readonly SpaceTemplate Template_5 => ref template_5;
    public static ref readonly SpaceTemplate Template_6 => ref template_6;
    public static ref readonly SpaceTemplate Template_7 => ref template_7;
    public static ref readonly SpaceTemplate Template_8 => ref template_8;
}