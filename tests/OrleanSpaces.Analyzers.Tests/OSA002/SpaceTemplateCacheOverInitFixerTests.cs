using OrleanSpaces.Analyzers.OSA002;

namespace OrleanSpaces.Analyzers.Tests.OSA002;

public class SpaceTemplateCacheOverInitFixerTests : FixerFixture
{
    public SpaceTemplateCacheOverInitFixerTests() : base(
        new SpaceTemplateCacheOverInitAnalyzer(),
        new SpaceTemplateCacheOverInitFixer(),
        SpaceTemplateCacheOverInitAnalyzer.Diagnostic.Id)
    {

    }

    [Fact]
    public void Should_Equal() =>
        Assert.Equal("OSA003", provider.FixableDiagnosticIds.Single());

    [Theory]
    [InlineData(1, "SpaceTemplate template = [|new()|];")]
    [InlineData(1, "SpaceTemplate template = [|new(SpaceUnit.Null)|];")]
    [InlineData(2, "SpaceTemplate template = [|new(SpaceUnit.Null, SpaceUnit.Null)|];")]
    [InlineData(4, "SpaceTemplate template = [|new(SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null)|];")]
    [InlineData(8, "SpaceTemplate template = [|new(SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null)|];")]
    public void Should_Fix_SpaceTemplate_Without_Namespace(int numOfSpaceUnits, string code)
    {
        TestCodeFix(code, GenerateFixedCode(numOfSpaceUnits), Namespace.OrleanSpaces_Tuples);
        static string GenerateFixedCode(int numOfSpaceUnits)
        {
            string unitArrayArgument = string.Join(", ", Enumerable.Repeat("SpaceUnit.Null", numOfSpaceUnits));

            return
    @$"SpaceTemplate template = SpaceTemplateCache.Tuple_{numOfSpaceUnits};

public readonly struct SpaceTemplateCache
{{
#pragma warning disable OSA003
    private static readonly SpaceTemplate tuple_{numOfSpaceUnits} = new({unitArrayArgument});
#pragma warning restore OSA003

    public static ref readonly SpaceTemplate Tuple_{numOfSpaceUnits} => ref tuple_{numOfSpaceUnits};
}}";
        }
    }

    [Theory]
    [InlineData(1, "namespace MyNamespace; SpaceTemplate template = [|new()|];")]
    [InlineData(1, "namespace MyNamespace; SpaceTemplate template = [|new(SpaceUnit.Null)|];")]
    [InlineData(2, "namespace MyNamespace; SpaceTemplate template = [|new(SpaceUnit.Null, SpaceUnit.Null)|];")]
    [InlineData(4, "namespace MyNamespace; SpaceTemplate template = [|new(SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null)|];")]
    [InlineData(8, "namespace MyNamespace; SpaceTemplate template = [|new(SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null)|];")]
    public void Should_Fix_SpaceTemplate_With_Namespace(int numOfSpaceUnits, string code)
    {
        TestCodeFix(code, GenerateFixedCode(numOfSpaceUnits), Namespace.OrleanSpaces_Tuples);
        static string GenerateFixedCode(int numOfSpaceUnits)
        {
            string unitArrayArgument = string.Join(", ", Enumerable.Repeat("SpaceUnit.Null", numOfSpaceUnits));

            return
    @$"namespace MyNamespace; SpaceTemplate template = SpaceTemplateCache.Tuple_{numOfSpaceUnits};

public readonly struct SpaceTemplateCache
{{
#pragma warning disable OSA003
    private static readonly SpaceTemplate tuple_{numOfSpaceUnits} = new({unitArrayArgument});
#pragma warning restore OSA003

    public static ref readonly SpaceTemplate Tuple_{numOfSpaceUnits} => ref tuple_{numOfSpaceUnits};
}}";
        }
    }

    [Fact]
    public void Should_Fix_1_Tuple_SpaceTemplate_By_Using_Existing_SpaceTemplateCache()
    {
        string code =
@"SpaceTemplate template = [|new(SpaceUnit.Null)|];

public readonly struct SpaceTemplateCache
{
#pragma warning disable OSA003
    private static readonly SpaceTemplate tuple_1 = new(SpaceUnit.Null);
#pragma warning restore OSA003

    public static ref readonly SpaceTemplate Tuple_1 => ref tuple_1;
}";

        string fix =
@"SpaceTemplate template = SpaceTemplateCache.Tuple_1;

public readonly struct SpaceTemplateCache
{
#pragma warning disable OSA003
    private static readonly SpaceTemplate tuple_1 = new(SpaceUnit.Null);
#pragma warning restore OSA003

    public static ref readonly SpaceTemplate Tuple_1 => ref tuple_1;
}";

        TestCodeFix(code, fix, Namespace.OrleanSpaces_Tuples);
    }

    [Fact]
    public void Should_Fix_2_Tuple_SpaceTemplate_By_Using_Existing_SpaceTemplateCache()
    {
        string code =
@"SpaceTemplate template = [|new(SpaceUnit.Null, SpaceUnit.Null)|];

public readonly struct SpaceTemplateCache
{
#pragma warning disable OSA003
    private static readonly SpaceTemplate tuple_2 = new(SpaceUnit.Null, SpaceUnit.Null);
#pragma warning restore OSA003

    public static ref readonly SpaceTemplate Tuple_2 => ref tuple_2;
}";

        string fix =
@"SpaceTemplate template = SpaceTemplateCache.Tuple_2;

public readonly struct SpaceTemplateCache
{
#pragma warning disable OSA003
    private static readonly SpaceTemplate tuple_2 = new(SpaceUnit.Null, SpaceUnit.Null);
#pragma warning restore OSA003

    public static ref readonly SpaceTemplate Tuple_2 => ref tuple_2;
}";

        TestCodeFix(code, fix, Namespace.OrleanSpaces_Tuples);
    }

    [Fact]
    public void Should_Fix_2_Tuple_SpaceTemplate_With_FullyQualifiedName_By_Using_Existing_SpaceTemplateCache()
    {
        string code =
@"SpaceTemplate template = [|new(SpaceUnit.Null, SpaceUnit.Null)|];

public readonly struct SpaceTemplateCache
{
#pragma warning disable OSA003
    private static readonly OrleanSpaces.Tuples.SpaceTemplate tuple_2 = new (SpaceUnit.Null, SpaceUnit.Null);
#pragma warning restore OSA003

    public static ref readonly SpaceTemplate Tuple_2 => ref tuple_2;
}";

        string fix =
@"SpaceTemplate template = SpaceTemplateCache.Tuple_2;

public readonly struct SpaceTemplateCache
{
#pragma warning disable OSA003
    private static readonly OrleanSpaces.Tuples.SpaceTemplate tuple_2 = new (SpaceUnit.Null, SpaceUnit.Null);
#pragma warning restore OSA003

    public static ref readonly SpaceTemplate Tuple_2 => ref tuple_2;
}";

        TestCodeFix(code, fix, Namespace.OrleanSpaces_Tuples);
    }

    [Fact]
    public void Should_Fix_2_Tuple_SpaceTemplate_By_Adding_Field_Between_1_And_3_In_Existing_SpaceTemplateCache()
    {
        string code =
@"SpaceTemplate template = [|new(SpaceUnit.Null, SpaceUnit.Null)|];

public readonly struct SpaceTemplateCache
{
#pragma warning disable OSA003
    private static readonly SpaceTemplate tuple_1 = new(SpaceUnit.Null);
    private static readonly SpaceTemplate tuple_3 = new(SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null);
#pragma warning restore OSA003

    public static ref readonly SpaceTemplate Tuple_1 => ref tuple_1;
    public static ref readonly SpaceTemplate Tuple_3 => ref tuple_3;
}";

        string fix =
@"SpaceTemplate template = SpaceTemplateCache.Tuple_2;

public readonly struct SpaceTemplateCache
{
#pragma warning disable OSA003
    private static readonly SpaceTemplate tuple_1 = new(SpaceUnit.Null);
    private static readonly SpaceTemplate tuple_2 = new(SpaceUnit.Null, SpaceUnit.Null);
    private static readonly SpaceTemplate tuple_3 = new(SpaceUnit.Null, SpaceUnit.Null, SpaceUnit.Null);
#pragma warning restore OSA003

    public static ref readonly SpaceTemplate Tuple_1 => ref tuple_1;
    public static ref readonly SpaceTemplate Tuple_2 => ref tuple_2;
    public static ref readonly SpaceTemplate Tuple_3 => ref tuple_3;
}";

        TestCodeFix(code, fix, Namespace.OrleanSpaces_Tuples);
    }
}
