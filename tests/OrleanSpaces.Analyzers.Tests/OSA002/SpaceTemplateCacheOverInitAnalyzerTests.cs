using OrleanSpaces.Analyzers.OSA002;

namespace OrleanSpaces.Analyzers.Tests.OSA002;

public class SpaceTemplateCacheOverInitAnalyzerTests : AnalyzerFixture
{
    public SpaceTemplateCacheOverInitAnalyzerTests() : base(
        new SpaceTemplateCacheOverInitAnalyzer(),
        SpaceTemplateCacheOverInitAnalyzer.Diagnostic.Id)
    {

    }

    [Fact]
    public void Should_Equal()
    {
        var diagnostic = SpaceTemplateCacheOverInitAnalyzer.Diagnostic;

        Assert.Equal("OSA002", diagnostic.Id);
        Assert.Equal(Categories.Performance, diagnostic.Category);
        Assert.Equal(DiagnosticSeverity.Info, diagnostic.DefaultSeverity);
        Assert.Equal("Avoid constructor instantiation of 'SpaceTemplate' having only 'SpaceUnit' type arguments.", diagnostic.Title);
        Assert.Equal("Avoid constructor instantiation of 'SpaceTemplate' having only 'SpaceUnit' type arguments.", diagnostic.MessageFormat);
        Assert.True(diagnostic.IsEnabledByDefault);
    }

    [Theory]
    [InlineData("SpaceTemplate template = new([||]);")]
    [InlineData("SpaceTemplate template = new([|new SpaceUnit()|]);")]
    [InlineData("SpaceTemplate template = new([|new SpaceUnit(), new SpaceUnit()|]);")]
    [InlineData("SpaceTemplate template = new([|SpaceUnit.Null|]);")]
    [InlineData("SpaceTemplate template = new([|SpaceUnit.Null, SpaceUnit.Null|]);")]
    [InlineData("SpaceTemplate template = new([|SpaceUnit.Null, new SpaceUnit(), SpaceUnit.Null|]);")]
    public void Should_Diagnose_SpaceTemplate(string code) =>
        HasDiagnostic(code, Namespace.OrleanSpaces_Tuples);
}
