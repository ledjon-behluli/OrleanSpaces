using OrleanSpaces.Analyzers.OSA002;

namespace OrleanSpaces.Analyzers.Tests.OSA002;

public class TemplateCacheOverInitFixerTests : FixerFixture
{
    public TemplateCacheOverInitFixerTests() : base(
        new TemplateCacheOverInitAnalyzer(),
        new TemplateCacheOverInitFixer(),
        TemplateCacheOverInitAnalyzer.Diagnostic.Id)
    {

    }

    [Fact]
    public void Should_Equal() =>
        Assert.Equal("OSA002", provider.FixableDiagnosticIds.Single());

    #region Non-Existing SpaceTemplateCache 
    
    private static (string, string) GetNestedActionTitle(int numOfNulls, bool isNewFile) =>
        new("Cache value as a static readonly reference", isNewFile ?
            $"Cache value as a '{numOfNulls}-tuple' static readonly reference in a new file" :
            $"Cache value as a '{numOfNulls}-tuple' static readonly reference in this file");

    #region Within File

    [Theory]
    [InlineData(1, "SpaceTemplate template = [|new(null)|];")]
    [InlineData(2, "SpaceTemplate template = [|new(null, null)|];")]
    [InlineData(4, "SpaceTemplate template = [|new(null, null, null, null)|];")]
    [InlineData(8, "SpaceTemplate template = [|new(null, null, null, null, null, null, null, null)|];")]
    public void Should_Fix_Space_Template_Without_Namespace_Within_File(int numOfNulls, string code)
    {
        var (groupTitle, actionTitle) = GetNestedActionTitle(numOfNulls, isNewFile: false);
        TestCodeFix(groupTitle, actionTitle, code, GenerateFixedCodeWithinFile("SpaceTemplate", numOfNulls, useNamespace: false), Namespace.OrleanSpaces_Tuples);
    }

    [Theory]
    [InlineData(1, "IntTemplate template = [|new(null)|];")]
    [InlineData(2, "IntTemplate template = [|new(null, null)|];")]
    [InlineData(4, "IntTemplate template = [|new(null, null, null, null)|];")]
    [InlineData(8, "IntTemplate template = [|new(null, null, null, null, null, null, null, null)|];")]
    public void Should_Fix_Specialized_Template_Without_Namespace_Within_File(int numOfNulls, string code)
    {
        var (groupTitle, actionTitle) = GetNestedActionTitle(numOfNulls, isNewFile: false);
        TestCodeFix(groupTitle, actionTitle, code, GenerateFixedCodeWithinFile("IntTemplate", numOfNulls, useNamespace: false), Namespace.OrleanSpaces_Tuples_Specialized);
    }

    [Theory]
    [InlineData(1, "namespace MyNamespace; SpaceTemplate template = [|new(null)|];")]
    [InlineData(2, "namespace MyNamespace; SpaceTemplate template = [|new(null, null)|];")]
    [InlineData(4, "namespace MyNamespace; SpaceTemplate template = [|new(null, null, null, null)|];")]
    [InlineData(8, "namespace MyNamespace; SpaceTemplate template = [|new(null, null, null, null, null, null, null, null)|];")]
    public void Should_Fix_Space_Template_With_Namespace_Within_File(int numOfNulls, string code)
    {
        var (groupTitle, actionTitle) = GetNestedActionTitle(numOfNulls, isNewFile: false);
        TestCodeFix(groupTitle, actionTitle, code, GenerateFixedCodeWithinFile("SpaceTemplate", numOfNulls, useNamespace: true), Namespace.OrleanSpaces_Tuples);
    }

    [Theory]
    [InlineData(1, "namespace MyNamespace; IntTemplate template = [|new(null)|];")]
    [InlineData(2, "namespace MyNamespace; IntTemplate template = [|new(null, null)|];")]
    [InlineData(4, "namespace MyNamespace; IntTemplate template = [|new(null, null, null, null)|];")]
    [InlineData(8, "namespace MyNamespace; IntTemplate template = [|new(null, null, null, null, null, null, null, null)|];")]
    public void Should_Fix_Specialized_Template_With_Namespace_Within_File(int numOfNulls, string code)
    {
        var (groupTitle, actionTitle) = GetNestedActionTitle(numOfNulls, isNewFile: false);
        TestCodeFix(groupTitle, actionTitle, code, GenerateFixedCodeWithinFile("IntTemplate", numOfNulls, useNamespace: true), Namespace.OrleanSpaces_Tuples_Specialized);
    }

    private static string GenerateFixedCodeWithinFile(string templateTypeName, int numOfNulls, bool useNamespace)
    {
        string unitArrayArgument = string.Join(", ", Enumerable.Repeat("null", numOfNulls));
        string potentialNamespace = useNamespace ? "namespace MyNamespace; " : string.Empty;

        return
@$"{potentialNamespace}{templateTypeName} template = {templateTypeName}Cache.Tuple_{numOfNulls};

public readonly struct {templateTypeName}Cache
{{
#pragma warning disable OSA002
    private static readonly {templateTypeName} tuple_{numOfNulls} = new({unitArrayArgument});
#pragma warning restore OSA002

    public static ref readonly {templateTypeName} Tuple_{numOfNulls} => ref tuple_{numOfNulls};
}}";
    }

    #endregion

    #region New File

    [Theory]
    [InlineData(1, "SpaceTemplate template = [|new()|];")]
    [InlineData(1, "SpaceTemplate template = [|new(null)|];")]
    [InlineData(2, "SpaceTemplate template = [|new(null, null)|];")]
    [InlineData(4, "SpaceTemplate template = [|new(null, null, null, null)|];")]
    [InlineData(8, "SpaceTemplate template = [|new(null, null, null, null, null, null, null, null)|];")]
    public void Should_Fix_Template_Without_Namespace_In_New_File(int numOfNulls, string code)
    {
        var (groupTitle, actionTitle) = GetNestedActionTitle(numOfNulls, isNewFile: true);
        TestCodeFix(groupTitle, actionTitle, code, GenerateFixedCodeNewFile(numOfNulls, useNamespace: false), Namespace.OrleanSpaces_Tuples);
    }

    [Theory]
    [InlineData(1, "namespace MyNamespace; SpaceTemplate template = [|new()|];")]
    [InlineData(1, "namespace MyNamespace; SpaceTemplate template = [|new(null)|];")]
    [InlineData(2, "namespace MyNamespace; SpaceTemplate template = [|new(null, null)|];")]
    [InlineData(4, "namespace MyNamespace; SpaceTemplate template = [|new(null, null, null, null)|];")]
    [InlineData(8, "namespace MyNamespace; SpaceTemplate template = [|new(null, null, null, null, null, null, null, null)|];")]
    public void Should_Fix_Template_With_Namespace_In_New_File(int numOfNulls, string code)
    {
        var (groupTitle, actionTitle) = GetNestedActionTitle(numOfNulls, isNewFile: true);
        TestCodeFix(groupTitle, actionTitle, code, GenerateFixedCodeNewFile(numOfNulls, useNamespace: true), Namespace.OrleanSpaces_Tuples);
    }

    private static string GenerateFixedCodeNewFile(int numOfNulls, bool useNamespace)
    {
        string potentialNamespace = useNamespace ? "namespace MyNamespace; " : string.Empty;
        return $"{potentialNamespace}SpaceTemplate template = SpaceTemplateCache.Tuple_{numOfNulls};";
    }

    #endregion

    #endregion

    #region Existing SpaceTemplateCache 

    [Theory]
    [InlineData("SpaceTemplate", Namespace.OrleanSpaces_Tuples)]
    [InlineData("IntTemplate", Namespace.OrleanSpaces_Tuples_Specialized)]
    public void Should_Fix_1_Tuple_Template_By_Using_Existing_TemplateCache(string templateTypeName, Namespace @namespace)
    {
        string code =
@$"{templateTypeName} template = [|new(null)|];

public readonly struct {templateTypeName}Cache
{{
#pragma warning disable OSA002
    private static readonly {templateTypeName} tuple_1 = new(null);
#pragma warning restore OSA002

    public static ref readonly {templateTypeName} Tuple_1 => ref tuple_1;
}}";

        string fix =
@$"{templateTypeName} template = {templateTypeName}Cache.Tuple_1;

public readonly struct {templateTypeName}Cache
{{
#pragma warning disable OSA002
    private static readonly {templateTypeName} tuple_1 = new(null);
#pragma warning restore OSA002

    public static ref readonly {templateTypeName} Tuple_1 => ref tuple_1;
}}";

        TestCodeFix(code, fix, @namespace);
    }

    [Theory]
    [InlineData("SpaceTemplate", Namespace.OrleanSpaces_Tuples)]
    [InlineData("IntTemplate", Namespace.OrleanSpaces_Tuples_Specialized)]
    public void Should_Fix_2_Tuple_Template_By_Using_Existing_TemplateCache(string templateTypeName, Namespace @namespace)
    {
        string code =
@$"{templateTypeName} template = [|new(null, null)|];

public readonly struct {templateTypeName}Cache
{{
#pragma warning disable OSA002
    private static readonly {templateTypeName} tuple_2 = new(null, null);
#pragma warning restore OSA002

    public static ref readonly {templateTypeName} Tuple_2 => ref tuple_2;
}}";

        string fix =
@$"{templateTypeName} template = {templateTypeName}Cache.Tuple_2;

public readonly struct {templateTypeName}Cache
{{
#pragma warning disable OSA002
    private static readonly {templateTypeName} tuple_2 = new(null, null);
#pragma warning restore OSA002

    public static ref readonly {templateTypeName} Tuple_2 => ref tuple_2;
}}";

        TestCodeFix(code, fix, @namespace);
    }

    [Theory]
    [InlineData("SpaceTemplate", Namespace.OrleanSpaces_Tuples)]
    [InlineData("IntTemplate", Namespace.OrleanSpaces_Tuples_Specialized)]
    public void Should_Fix_2_Tuple_Template_With_FullyQualifiedName_By_Using_Existing_TemplateCache(string templateTypeName, Namespace @namespace)
    {
        string code =
@$"{templateTypeName} template = [|new(null, null)|];

public readonly struct {templateTypeName}Cache
{{
#pragma warning disable OSA002
    private static readonly OrleanSpaces.Tuples.{templateTypeName} tuple_2 = new (null, null);
#pragma warning restore OSA002

    public static ref readonly {templateTypeName} Tuple_2 => ref tuple_2;
}}";

        string fix =
@$"{templateTypeName} template = {templateTypeName}Cache.Tuple_2;

public readonly struct {templateTypeName}Cache
{{
#pragma warning disable OSA002
    private static readonly OrleanSpaces.Tuples.{templateTypeName} tuple_2 = new (null, null);
#pragma warning restore OSA002

    public static ref readonly {templateTypeName} Tuple_2 => ref tuple_2;
}}";

        TestCodeFix(code, fix, @namespace);
    }

    [Theory]
    [InlineData("SpaceTemplate", Namespace.OrleanSpaces_Tuples)]
    [InlineData("IntTemplate", Namespace.OrleanSpaces_Tuples_Specialized)]
    public void Should_Fix_2_Tuple_Template_By_Adding_Field_Between_1_And_3_In_Existing_TemplateCache(string templateTypeName, Namespace @namespace)
    {
        string code =
@$"{templateTypeName} template = [|new(null, null)|];

public readonly struct {templateTypeName}Cache
{{
#pragma warning disable OSA002
    private static readonly {templateTypeName} tuple_1 = new(null);
    private static readonly {templateTypeName} tuple_3 = new(null, null, null);
#pragma warning restore OSA002

    public static ref readonly {templateTypeName} Tuple_1 => ref tuple_1;
    public static ref readonly {templateTypeName} Tuple_3 => ref tuple_3;
}}";

        string fix =
@$"{templateTypeName} template = {templateTypeName}Cache.Tuple_2;

public readonly struct {templateTypeName}Cache
{{
#pragma warning disable OSA002
    private static readonly {templateTypeName} tuple_1 = new(null);
    private static readonly {templateTypeName} tuple_2 = new(null, null);
    private static readonly {templateTypeName} tuple_3 = new(null, null, null);
#pragma warning restore OSA002

    public static ref readonly {templateTypeName} Tuple_1 => ref tuple_1;
    public static ref readonly {templateTypeName} Tuple_2 => ref tuple_2;
    public static ref readonly {templateTypeName} Tuple_3 => ref tuple_3;
}}";

        TestCodeFix(code, fix, @namespace);
    }

    #endregion
}
