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
        Assert.Equal("OSA002", provider.FixableDiagnosticIds.Single());

    #region Non-Existing SpaceTemplateCache 

    private static (string, string) GetNestedActionTitle(int numOfSpaceUnits, bool isNewFile) =>
        new("Cache value as a static readonly reference", isNewFile ?
            $"Cache value as a '{numOfSpaceUnits}-tuple' static readonly reference in a new file" :
            $"Cache value as a '{numOfSpaceUnits}-tuple' static readonly reference in this file");

    #region Within File

    [Theory]
    [InlineData(1, "SpaceTemplate template = [|new()|];")]
    [InlineData(1, "SpaceTemplate template = [|new(new SpaceUnit())|];")]
    [InlineData(2, "SpaceTemplate template = [|new(new SpaceUnit(), new SpaceUnit())|];")]
    [InlineData(4, "SpaceTemplate template = [|new(new SpaceUnit(), new SpaceUnit(), new SpaceUnit(), new SpaceUnit())|];")]
    [InlineData(8, "SpaceTemplate template = [|new(new SpaceUnit(), new SpaceUnit(), new SpaceUnit(), new SpaceUnit(), new SpaceUnit(), new SpaceUnit(), new SpaceUnit(), new SpaceUnit())|];")]
    public void Should_Fix_SpaceTemplate_Without_Namespace_Within_File(int numOfSpaceUnits, string code)
    {
        var (groupTitle, actionTitle) = GetNestedActionTitle(numOfSpaceUnits, isNewFile: false);
        TestCodeFix(groupTitle, actionTitle, code, GenerateFixedCodeWithinFile(numOfSpaceUnits, useNamespace: false), Namespace.OrleanSpaces_Tuples);
    }

    [Theory]
    [InlineData(1, "namespace MyNamespace; SpaceTemplate template = [|new()|];")]
    [InlineData(1, "namespace MyNamespace; SpaceTemplate template = [|new(new SpaceUnit())|];")]
    [InlineData(2, "namespace MyNamespace; SpaceTemplate template = [|new(new SpaceUnit(), new SpaceUnit())|];")]
    [InlineData(4, "namespace MyNamespace; SpaceTemplate template = [|new(new SpaceUnit(), new SpaceUnit(), new SpaceUnit(), new SpaceUnit())|];")]
    [InlineData(8, "namespace MyNamespace; SpaceTemplate template = [|new(new SpaceUnit(), new SpaceUnit(), new SpaceUnit(), new SpaceUnit(), new SpaceUnit(), new SpaceUnit(), new SpaceUnit(), new SpaceUnit())|];")]
    public void Should_Fix_SpaceTemplate_With_Namespace_Within_File(int numOfSpaceUnits, string code)
    {
        var (groupTitle, actionTitle) = GetNestedActionTitle(numOfSpaceUnits, isNewFile: false);
        TestCodeFix(groupTitle, actionTitle, code, GenerateFixedCodeWithinFile(numOfSpaceUnits, useNamespace: true), Namespace.OrleanSpaces_Tuples);
    }

    private static string GenerateFixedCodeWithinFile(int numOfSpaceUnits, bool useNamespace)
    {
        string unitArrayArgument = string.Join(", ", Enumerable.Repeat("new SpaceUnit()", numOfSpaceUnits));
        string potentialNamespace = useNamespace ? "namespace MyNamespace; " : string.Empty;

        return
@$"{potentialNamespace}SpaceTemplate template = SpaceTemplateCache.Tuple_{numOfSpaceUnits};

public readonly struct SpaceTemplateCache
{{
#pragma warning disable OSA002
    private static readonly SpaceTemplate tuple_{numOfSpaceUnits} = new({unitArrayArgument});
#pragma warning restore OSA002

    public static ref readonly SpaceTemplate Tuple_{numOfSpaceUnits} => ref tuple_{numOfSpaceUnits};
}}";
    }

    #endregion

    #region New File

    [Theory]
    [InlineData(1, "SpaceTemplate template = [|new()|];")]
    [InlineData(1, "SpaceTemplate template = [|new(new SpaceUnit())|];")]
    [InlineData(2, "SpaceTemplate template = [|new(new SpaceUnit(), new SpaceUnit())|];")]
    [InlineData(4, "SpaceTemplate template = [|new(new SpaceUnit(), new SpaceUnit(), new SpaceUnit(), new SpaceUnit())|];")]
    [InlineData(8, "SpaceTemplate template = [|new(new SpaceUnit(), new SpaceUnit(), new SpaceUnit(), new SpaceUnit(), new SpaceUnit(), new SpaceUnit(), new SpaceUnit(), new SpaceUnit())|];")]
    public void Should_Fix_SpaceTemplate_Without_Namespace_In_New_File(int numOfSpaceUnits, string code)
    {
        var (groupTitle, actionTitle) = GetNestedActionTitle(numOfSpaceUnits, isNewFile: true);
        TestCodeFix(groupTitle, actionTitle, code, GenerateFixedCodeNewFile(numOfSpaceUnits, useNamespace: false), Namespace.OrleanSpaces_Tuples);
    }

    [Theory]
    [InlineData(1, "namespace MyNamespace; SpaceTemplate template = [|new()|];")]
    [InlineData(1, "namespace MyNamespace; SpaceTemplate template = [|new(new SpaceUnit())|];")]
    [InlineData(2, "namespace MyNamespace; SpaceTemplate template = [|new(new SpaceUnit(), new SpaceUnit())|];")]
    [InlineData(4, "namespace MyNamespace; SpaceTemplate template = [|new(new SpaceUnit(), new SpaceUnit(), new SpaceUnit(), new SpaceUnit())|];")]
    [InlineData(8, "namespace MyNamespace; SpaceTemplate template = [|new(new SpaceUnit(), new SpaceUnit(), new SpaceUnit(), new SpaceUnit(), new SpaceUnit(), new SpaceUnit(), new SpaceUnit(), new SpaceUnit())|];")]
    public void Should_Fix_SpaceTemplate_With_Namespace_In_New_File(int numOfSpaceUnits, string code)
    {
        var (groupTitle, actionTitle) = GetNestedActionTitle(numOfSpaceUnits, isNewFile: true);
        TestCodeFix(groupTitle, actionTitle, code, GenerateFixedCodeNewFile(numOfSpaceUnits, useNamespace: true), Namespace.OrleanSpaces_Tuples);
    }

    private static string GenerateFixedCodeNewFile(int numOfSpaceUnits, bool useNamespace)
    {
        string potentialNamespace = useNamespace ? "namespace MyNamespace; " : string.Empty;
        return $"{potentialNamespace}SpaceTemplate template = SpaceTemplateCache.Tuple_{numOfSpaceUnits};";
    }

    #endregion

    #endregion

    #region Existing SpaceTemplateCache 

    [Fact]
    public void Should_Fix_1_Tuple_SpaceTemplate_By_Using_Existing_SpaceTemplateCache()
    {
        string code =
@"SpaceTemplate template = [|new(new SpaceUnit())|];

public readonly struct SpaceTemplateCache
{
#pragma warning disable OSA002
    private static readonly SpaceTemplate tuple_1 = new(new SpaceUnit());
#pragma warning restore OSA002

    public static ref readonly SpaceTemplate Tuple_1 => ref tuple_1;
}";

        string fix =
@"SpaceTemplate template = SpaceTemplateCache.Tuple_1;

public readonly struct SpaceTemplateCache
{
#pragma warning disable OSA002
    private static readonly SpaceTemplate tuple_1 = new(new SpaceUnit());
#pragma warning restore OSA002

    public static ref readonly SpaceTemplate Tuple_1 => ref tuple_1;
}";

        TestCodeFix(code, fix, Namespace.OrleanSpaces_Tuples);
    }

    [Fact]
    public void Should_Fix_2_Tuple_SpaceTemplate_By_Using_Existing_SpaceTemplateCache()
    {
        string code =
@"SpaceTemplate template = [|new(new SpaceUnit(), new SpaceUnit())|];

public readonly struct SpaceTemplateCache
{
#pragma warning disable OSA002
    private static readonly SpaceTemplate tuple_2 = new(new SpaceUnit(), new SpaceUnit());
#pragma warning restore OSA002

    public static ref readonly SpaceTemplate Tuple_2 => ref tuple_2;
}";

        string fix =
@"SpaceTemplate template = SpaceTemplateCache.Tuple_2;

public readonly struct SpaceTemplateCache
{
#pragma warning disable OSA002
    private static readonly SpaceTemplate tuple_2 = new(new SpaceUnit(), new SpaceUnit());
#pragma warning restore OSA002

    public static ref readonly SpaceTemplate Tuple_2 => ref tuple_2;
}";

        TestCodeFix(code, fix, Namespace.OrleanSpaces_Tuples);
    }

    [Fact]
    public void Should_Fix_2_Tuple_SpaceTemplate_With_FullyQualifiedName_By_Using_Existing_SpaceTemplateCache()
    {
        string code =
@"SpaceTemplate template = [|new(new SpaceUnit(), new SpaceUnit())|];

public readonly struct SpaceTemplateCache
{
#pragma warning disable OSA002
    private static readonly OrleanSpaces.Tuples.SpaceTemplate tuple_2 = new (new SpaceUnit(), new SpaceUnit());
#pragma warning restore OSA002

    public static ref readonly SpaceTemplate Tuple_2 => ref tuple_2;
}";

        string fix =
@"SpaceTemplate template = SpaceTemplateCache.Tuple_2;

public readonly struct SpaceTemplateCache
{
#pragma warning disable OSA002
    private static readonly OrleanSpaces.Tuples.SpaceTemplate tuple_2 = new (new SpaceUnit(), new SpaceUnit());
#pragma warning restore OSA002

    public static ref readonly SpaceTemplate Tuple_2 => ref tuple_2;
}";

        TestCodeFix(code, fix, Namespace.OrleanSpaces_Tuples);
    }

    [Fact]
    public void Should_Fix_2_Tuple_SpaceTemplate_By_Adding_Field_Between_1_And_3_In_Existing_SpaceTemplateCache()
    {
        string code =
@"SpaceTemplate template = [|new(new SpaceUnit(), new SpaceUnit())|];

public readonly struct SpaceTemplateCache
{
#pragma warning disable OSA002
    private static readonly SpaceTemplate tuple_1 = new(new SpaceUnit());
    private static readonly SpaceTemplate tuple_3 = new(new SpaceUnit(), new SpaceUnit(), new SpaceUnit());
#pragma warning restore OSA002

    public static ref readonly SpaceTemplate Tuple_1 => ref tuple_1;
    public static ref readonly SpaceTemplate Tuple_3 => ref tuple_3;
}";

        string fix =
@"SpaceTemplate template = SpaceTemplateCache.Tuple_2;

public readonly struct SpaceTemplateCache
{
#pragma warning disable OSA002
    private static readonly SpaceTemplate tuple_1 = new(new SpaceUnit());
    private static readonly SpaceTemplate tuple_2 = new(new SpaceUnit(), new SpaceUnit());
    private static readonly SpaceTemplate tuple_3 = new(new SpaceUnit(), new SpaceUnit(), new SpaceUnit());
#pragma warning restore OSA002

    public static ref readonly SpaceTemplate Tuple_1 => ref tuple_1;
    public static ref readonly SpaceTemplate Tuple_2 => ref tuple_2;
    public static ref readonly SpaceTemplate Tuple_3 => ref tuple_3;
}";

        TestCodeFix(code, fix, Namespace.OrleanSpaces_Tuples);
    }

    #endregion
}
