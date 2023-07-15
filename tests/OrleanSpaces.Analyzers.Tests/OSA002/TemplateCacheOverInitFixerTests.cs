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

    #region Non-Existing {X}TemplateCache 

    #region Within File

    [Theory]

    [InlineData(1, Namespace.OrleanSpaces_Tuples, "SpaceTemplate template = [|new(null)|];")]
    [InlineData(2, Namespace.OrleanSpaces_Tuples, "SpaceTemplate template = [|new(null, null)|];")]
    [InlineData(4, Namespace.OrleanSpaces_Tuples, "SpaceTemplate template = [|new(null, null, null, null)|];")]
    [InlineData(8, Namespace.OrleanSpaces_Tuples, "SpaceTemplate template = [|new(null, null, null, null, null, null, null, null)|];")]

    [InlineData(1, Namespace.OrleanSpaces_Tuples_Specialized, "IntTemplate template = [|new(null)|];")]
    [InlineData(2, Namespace.OrleanSpaces_Tuples_Specialized, "IntTemplate template = [|new(null, null)|];")]
    [InlineData(4, Namespace.OrleanSpaces_Tuples_Specialized, "IntTemplate template = [|new(null, null, null, null)|];")]
    [InlineData(8, Namespace.OrleanSpaces_Tuples_Specialized, "IntTemplate template = [|new(null, null, null, null, null, null, null, null)|];")]
    public void Should_Fix_Template_Without_Namespace_Within_File(int numOfNulls, Namespace @namespace, string code)
    {
        string templateTypeName = code.Split(' ')[0];
        string fix = GenerateFixedCodeWithinFile(templateTypeName, numOfNulls, useNamespace: false);
        var (groupTitle, actionTitle) = GetNestedActionTitle(numOfNulls, isNewFile: false);
       
        TestCodeFix(groupTitle, actionTitle, code, fix, @namespace);
    }

    [Theory]

    [InlineData(1, Namespace.OrleanSpaces_Tuples, "namespace MyNamespace; SpaceTemplate template = [|new(null)|];")]
    [InlineData(2, Namespace.OrleanSpaces_Tuples, "namespace MyNamespace; SpaceTemplate template = [|new(null, null)|];")]
    [InlineData(4, Namespace.OrleanSpaces_Tuples, "namespace MyNamespace; SpaceTemplate template = [|new(null, null, null, null)|];")]
    [InlineData(8, Namespace.OrleanSpaces_Tuples, "namespace MyNamespace; SpaceTemplate template = [|new(null, null, null, null, null, null, null, null)|];")]

    [InlineData(1, Namespace.OrleanSpaces_Tuples_Specialized, "namespace MyNamespace; IntTemplate template = [|new(null)|];")]
    [InlineData(2, Namespace.OrleanSpaces_Tuples_Specialized, "namespace MyNamespace; IntTemplate template = [|new(null, null)|];")]
    [InlineData(4, Namespace.OrleanSpaces_Tuples_Specialized, "namespace MyNamespace; IntTemplate template = [|new(null, null, null, null)|];")]
    [InlineData(8, Namespace.OrleanSpaces_Tuples_Specialized, "namespace MyNamespace; IntTemplate template = [|new(null, null, null, null, null, null, null, null)|];")]
    public void Should_Fix_Template_With_Namespace_Within_File(int numOfNulls, Namespace @namespace, string code)
    {
        string templateTypeName = code.Split(' ')[0];
        string fix = GenerateFixedCodeWithinFile(templateTypeName, numOfNulls, useNamespace: true);
        var (groupTitle, actionTitle) = GetNestedActionTitle(numOfNulls, isNewFile: false);

        TestCodeFix(groupTitle, actionTitle, code, fix, @namespace);
    }

    private static string GenerateFixedCodeWithinFile(string templateTypeName, int numOfNulls, bool useNamespace)
    {
        string nullArrayArgument = string.Join(", ", Enumerable.Repeat("null", numOfNulls));
        string potentialNamespace = useNamespace ? "namespace MyNamespace; " : string.Empty;

        return
@$"{potentialNamespace}{templateTypeName} template = {templateTypeName}Cache.Template_{numOfNulls};

public readonly struct {templateTypeName}Cache
{{
#pragma warning disable OSA002
    private static readonly {templateTypeName} template_{numOfNulls} = new({nullArrayArgument});
#pragma warning restore OSA002

    public static ref readonly {templateTypeName} Template_{numOfNulls} => ref template_{numOfNulls};
}}";
    }

    #endregion

    #region New File

    [Theory]

    [InlineData(1, Namespace.OrleanSpaces_Tuples, "SpaceTemplate template = [|new()|];")]
    [InlineData(1, Namespace.OrleanSpaces_Tuples, "SpaceTemplate template = [|new(null)|];")]
    [InlineData(2, Namespace.OrleanSpaces_Tuples, "SpaceTemplate template = [|new(null, null)|];")]
    [InlineData(4, Namespace.OrleanSpaces_Tuples, "SpaceTemplate template = [|new(null, null, null, null)|];")]
    [InlineData(8, Namespace.OrleanSpaces_Tuples, "SpaceTemplate template = [|new(null, null, null, null, null, null, null, null)|];")]

    [InlineData(1, Namespace.OrleanSpaces_Tuples_Specialized, "IntTemplate template = [|new()|];")]
    [InlineData(1, Namespace.OrleanSpaces_Tuples_Specialized, "IntTemplate template = [|new(null)|];")]
    [InlineData(2, Namespace.OrleanSpaces_Tuples_Specialized, "IntTemplate template = [|new(null, null)|];")]
    [InlineData(4, Namespace.OrleanSpaces_Tuples_Specialized, "IntTemplate template = [|new(null, null, null, null)|];")]
    [InlineData(8, Namespace.OrleanSpaces_Tuples_Specialized, "IntTemplate template = [|new(null, null, null, null, null, null, null, null)|];")]
    public void Should_Fix_Template_Without_Namespace_In_New_File(int numOfNulls, Namespace @namespace, string code)
    {
        string templateTypeName = code.Split(' ')[0];
        string fix = GenerateFixedCodeNewFile(templateTypeName, numOfNulls, useNamespace: false);
        var (groupTitle, actionTitle) = GetNestedActionTitle(numOfNulls, isNewFile: true);

        TestCodeFix(groupTitle, actionTitle, code, fix, @namespace);
    }

    [Theory]

    [InlineData(1, Namespace.OrleanSpaces_Tuples, "namespace MyNamespace; SpaceTemplate template = [|new()|];")]
    [InlineData(1, Namespace.OrleanSpaces_Tuples, "namespace MyNamespace; SpaceTemplate template = [|new(null)|];")]
    [InlineData(2, Namespace.OrleanSpaces_Tuples, "namespace MyNamespace; SpaceTemplate template = [|new(null, null)|];")]
    [InlineData(4, Namespace.OrleanSpaces_Tuples, "namespace MyNamespace; SpaceTemplate template = [|new(null, null, null, null)|];")]
    [InlineData(8, Namespace.OrleanSpaces_Tuples, "namespace MyNamespace; SpaceTemplate template = [|new(null, null, null, null, null, null, null, null)|];")]

    [InlineData(1, Namespace.OrleanSpaces_Tuples_Specialized, "namespace MyNamespace; IntTemplate template = [|new()|];")]
    [InlineData(1, Namespace.OrleanSpaces_Tuples_Specialized, "namespace MyNamespace; IntTemplate template = [|new(null)|];")]
    [InlineData(2, Namespace.OrleanSpaces_Tuples_Specialized, "namespace MyNamespace; IntTemplate template = [|new(null, null)|];")]
    [InlineData(4, Namespace.OrleanSpaces_Tuples_Specialized, "namespace MyNamespace; IntTemplate template = [|new(null, null, null, null)|];")]
    [InlineData(8, Namespace.OrleanSpaces_Tuples_Specialized, "namespace MyNamespace; IntTemplate template = [|new(null, null, null, null, null, null, null, null)|];")]
    public void Should_Fix_Template_With_Namespace_In_New_File(int numOfNulls, Namespace @namespace, string code)
    {
        string templateTypeName = code.Split(' ')[0];
        string fix = GenerateFixedCodeNewFile(templateTypeName, numOfNulls, useNamespace: true);
        var (groupTitle, actionTitle) = GetNestedActionTitle(numOfNulls, isNewFile: true);

        TestCodeFix(groupTitle, actionTitle, code, fix, @namespace);
    }

    private static string GenerateFixedCodeNewFile(string templateTypeName, int numOfNulls, bool useNamespace)
    {
        string potentialNamespace = useNamespace ? "namespace MyNamespace; " : string.Empty;
        return $"{potentialNamespace}{templateTypeName} template = {templateTypeName}Cache.Template_{numOfNulls};";
    }

    #endregion

    private static (string, string) GetNestedActionTitle(int numOfNulls, bool isNewFile) =>
        new("Cache value as a static readonly reference", isNewFile ?
            $"Cache value as a '{numOfNulls}-template' static readonly reference in a new file" :
            $"Cache value as a '{numOfNulls}-template' static readonly reference in this file");

    #endregion

    #region Existing {X}TemplateCache 

    [Theory]
    [InlineData("SpaceTemplate", Namespace.OrleanSpaces_Tuples)]
    [InlineData("IntTemplate", Namespace.OrleanSpaces_Tuples_Specialized)]
    public void Should_Fix_1_Template_By_Using_Existing_TemplateCache(string templateTypeName, Namespace @namespace)
    {
        string code =
@$"{templateTypeName} template = [|new(null)|];

public readonly struct {templateTypeName}Cache
{{
#pragma warning disable OSA002
    private static readonly {templateTypeName} template_1 = new(null);
#pragma warning restore OSA002

    public static ref readonly {templateTypeName} Template_1 => ref template_1;
}}";

        string fix =
@$"{templateTypeName} template = {templateTypeName}Cache.Template_1;

public readonly struct {templateTypeName}Cache
{{
#pragma warning disable OSA002
    private static readonly {templateTypeName} template_1 = new(null);
#pragma warning restore OSA002

    public static ref readonly {templateTypeName} Template_1 => ref template_1;
}}";

        TestCodeFix(code, fix, @namespace);
    }

    [Theory]
    [InlineData("SpaceTemplate", Namespace.OrleanSpaces_Tuples)]
    [InlineData("IntTemplate", Namespace.OrleanSpaces_Tuples_Specialized)]
    public void Should_Fix_2_Template_By_Using_Existing_TemplateCache(string templateTypeName, Namespace @namespace)
    {
        string code =
@$"{templateTypeName} template = [|new(null, null)|];

public readonly struct {templateTypeName}Cache
{{
#pragma warning disable OSA002
    private static readonly {templateTypeName} template_2 = new(null, null);
#pragma warning restore OSA002

    public static ref readonly {templateTypeName} Template_2 => ref template_2;
}}";

        string fix =
@$"{templateTypeName} template = {templateTypeName}Cache.Template_2;

public readonly struct {templateTypeName}Cache
{{
#pragma warning disable OSA002
    private static readonly {templateTypeName} template_2 = new(null, null);
#pragma warning restore OSA002

    public static ref readonly {templateTypeName} Template_2 => ref template_2;
}}";

        TestCodeFix(code, fix, @namespace);
    }

    [Theory]
    [InlineData("SpaceTemplate", Namespace.OrleanSpaces_Tuples)]
    [InlineData("IntTemplate", Namespace.OrleanSpaces_Tuples_Specialized)]
    public void Should_Fix_2_Template_With_FullyQualifiedName_By_Using_Existing_TemplateCache(string templateTypeName, Namespace @namespace)
    {
        string code =
@$"{templateTypeName} template = [|new(null, null)|];

public readonly struct {templateTypeName}Cache
{{
#pragma warning disable OSA002
    private static readonly OrleanSpaces.Tuples.{templateTypeName} template_2 = new (null, null);
#pragma warning restore OSA002

    public static ref readonly {templateTypeName} Template_2 => ref template_2;
}}";

        string fix =
@$"{templateTypeName} template = {templateTypeName}Cache.Template_2;

public readonly struct {templateTypeName}Cache
{{
#pragma warning disable OSA002
    private static readonly OrleanSpaces.Tuples.{templateTypeName} template_2 = new (null, null);
#pragma warning restore OSA002

    public static ref readonly {templateTypeName} Template_2 => ref template_2;
}}";

        TestCodeFix(code, fix, @namespace);
    }

    [Theory]
    [InlineData("SpaceTemplate", Namespace.OrleanSpaces_Tuples)]
    [InlineData("IntTemplate", Namespace.OrleanSpaces_Tuples_Specialized)]
    public void Should_Fix_2_Template_By_Adding_Field_Between_1_And_3_In_Existing_TemplateCache(string templateTypeName, Namespace @namespace)
    {
        string code =
@$"{templateTypeName} template = [|new(null, null)|];

public readonly struct {templateTypeName}Cache
{{
#pragma warning disable OSA002
    private static readonly {templateTypeName} template_1 = new(null);
    private static readonly {templateTypeName} template_3 = new(null, null, null);
#pragma warning restore OSA002

    public static ref readonly {templateTypeName} Template_1 => ref template_1;
    public static ref readonly {templateTypeName} Template_3 => ref template_3;
}}";

        string fix =
@$"{templateTypeName} template = {templateTypeName}Cache.Template_2;

public readonly struct {templateTypeName}Cache
{{
#pragma warning disable OSA002
    private static readonly {templateTypeName} template_1 = new(null);
    private static readonly {templateTypeName} template_2 = new(null, null);
    private static readonly {templateTypeName} template_3 = new(null, null, null);
#pragma warning restore OSA002

    public static ref readonly {templateTypeName} Template_1 => ref template_1;
    public static ref readonly {templateTypeName} Template_2 => ref template_2;
    public static ref readonly {templateTypeName} Template_3 => ref template_3;
}}";

        TestCodeFix(code, fix, @namespace);
    }

    #endregion
}
