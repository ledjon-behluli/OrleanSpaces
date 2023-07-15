using OrleanSpaces.Analyzers.OSA001;

namespace OrleanSpaces.Analyzers.Tests.OSA001;

public class InternalUseOnlyAttributeAnalyzerTests : AnalyzerFixture
{
    public InternalUseOnlyAttributeAnalyzerTests() : base(
        new InternalUseOnlyAttributeAnalyzer(),
        InternalUseOnlyAttributeAnalyzer.Diagnostic.Id)
    {

    }

    [Fact]
    public void Should_Equal()
    {
        var diagnostic = InternalUseOnlyAttributeAnalyzer.Diagnostic;

        Assert.Equal("OSA001", diagnostic.Id);
        Assert.Equal(Categories.Usage, diagnostic.Category);
        Assert.Equal(DiagnosticSeverity.Info, diagnostic.DefaultSeverity);
        Assert.Equal("Interface is intended for internal use only.", diagnostic.Title);
        Assert.Equal("Interface '{0}' is intended for internal use only.", diagnostic.MessageFormat);
        Assert.True(diagnostic.IsEnabledByDefault);
    }

    [Theory]
    [InlineData("[|ISpaceTuple tuple|]  = new SpaceTuple(1);")]
    [InlineData("[|ISpaceTemplate tuple|]  = new SpaceTemplate(1);")]

    [InlineData("[|ISpaceTuple<int>|]  tuple = new Tuple(1);")]
    [InlineData("[|ISpaceTemplate<int>|]  tuple = new IntTuple(1);")]
    public void Should_Diagnose(string code) =>
        HasDiagnostic(code, Namespace.MyNamespace);

    [Theory]
    [InlineData("[|ISpaceTuple tuple|]  = new SpaceTuple(1);")]
    [InlineData("[|ISpaceTemplate tuple|]  = new SpaceTemplate(1);")]
                 
    [InlineData("[|ISpaceTuple<int>|]  tuple = new Tuple(1);")]
    [InlineData("[|ISpaceTemplate<int>|]  tuple = new IntTuple(1);")]
    public void Should_Not_Diagnose(string code) =>
        NoDiagnostic(code, Namespace.OrleanSpaces);
}
