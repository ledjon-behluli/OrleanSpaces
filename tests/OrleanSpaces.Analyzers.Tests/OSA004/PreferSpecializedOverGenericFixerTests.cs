using OrleanSpaces.Analyzers.OSA004;

namespace OrleanSpaces.Analyzers.Tests.OSA004;

public class PreferSpecializedOverGenericFixerTests : FixerFixture
{
    public PreferSpecializedOverGenericFixerTests() : base(
        new PreferSpecializedOverGenericAnalyzer(),
        new PreferSpecializedOverGenericFixer(),
        PreferSpecializedOverGenericAnalyzer.Diagnostic.Id)
    {
        
    }

    [Fact]
    public void Should_Equal() =>
      Assert.Equal("OSA004", provider.FixableDiagnosticIds.Single());

    [Theory]
    [InlineData("[|var tuple = new SpaceTuple(1);|]", "Int")]
    [InlineData("[|SpaceTuple tuple = new SpaceTuple(1);|]", "Int")]
    [InlineData("[|SpaceTuple tuple = new(1);|]", "Int")]
    public void AAAA(string code, string specializedTypePrefix)
    {
        string fix = RemoveDiagnosticSpanFromText(code.Replace("Space", specializedTypePrefix));
        TestCodeFix(code, fix, Namespace.OrleanSpaces_Tuples, Namespace.OrleanSpaces_Tuples_Specialized);
    }

    [Theory]
    [InlineData("[|SpaceTuple tuple = new SpaceTuple(1);|]", "Int")]
    public void Should_Fix_ObjInit_SpaceTuple(string code, string specializedTypePrefix)
    {
        string fix = RemoveDiagnosticSpanFromText(code.Replace("Space", specializedTypePrefix));
        TestCodeFix(code, fix, Namespace.OrleanSpaces_Tuples, Namespace.OrleanSpaces_Tuples_Specialized);
    }

    [Theory]
    [InlineData("[|SpaceTuple tuple = new(1);|]", "Int")]
    public void Should_Fix_SimplifiedObjInit_SpaceTuple(string code, string specializedTypePrefix)
    {
        string fix = RemoveDiagnosticSpanFromText(code.Replace("Space", specializedTypePrefix));
        TestCodeFix(code, fix, Namespace.OrleanSpaces_Tuples, Namespace.OrleanSpaces_Tuples_Specialized);
    }

    [Theory]
    [InlineData("[|var tuple = new SpaceTuple(1);|]", "Int")]
    public void Should_Fix_VarInit_SpaceTuple(string code, string specializedTypePrefix)
    {
        string fix = RemoveDiagnosticSpanFromText(code.Replace("Space", specializedTypePrefix));
        TestCodeFix(code, fix, Namespace.OrleanSpaces_Tuples, Namespace.OrleanSpaces_Tuples_Specialized);
    }

//    [Theory]
//    [InlineData(
//@"class C
//{
//    public C()
//    {
//        [|SpaceTuple tuple = new(1);|]
//    }
//}"
//, "Int")]
    public void Should_Fix_SpaceTuple_1(string code, string specializedTypePrefix)
    {
        string fix = code.Replace("Space", specializedTypePrefix);
        TestCodeFix(code, fix, Namespace.OrleanSpaces_Tuples, Namespace.OrleanSpaces_Tuples_Specialized);
    }
}
