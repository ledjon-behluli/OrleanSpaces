using OrleanSpaces.Analyzers.OSA002;

namespace OrleanSpaces.Analyzers.Tests.OSA002;

public class TemplateCacheOverInitAnalyzerTests : AnalyzerFixture
{
    public TemplateCacheOverInitAnalyzerTests() : base(
        new TemplateCacheOverInitAnalyzer(),
        TemplateCacheOverInitAnalyzer.Diagnostic.Id)
    {

    }

    [Fact]
    public void Should_Equal()
    {
        var diagnostic = TemplateCacheOverInitAnalyzer.Diagnostic;

        Assert.Equal("OSA002", diagnostic.Id);
        Assert.Equal(Categories.Performance, diagnostic.Category);
        Assert.Equal(DiagnosticSeverity.Info, diagnostic.DefaultSeverity);
        Assert.Equal("Avoid constructor instantiation having only 'null' type, or no arguments.", diagnostic.Title);
        Assert.Equal("Avoid constructor instantiation of '{0}' having only 'null' type, or no arguments.", diagnostic.MessageFormat);
        Assert.True(diagnostic.IsEnabledByDefault);
    }

    [Theory]

    [InlineData(Namespace.OrleanSpaces_Tuples, "SpaceTemplate template = new([|null|]);")]
    [InlineData(Namespace.OrleanSpaces_Tuples, "SpaceTemplate template = new([|null, null|]);")]
    [InlineData(Namespace.OrleanSpaces_Tuples, "SpaceTemplate template = new([|null, null, null|]);")]

    [InlineData(Namespace.OrleanSpaces_Tuples_Specialized, "IntTemplate template = new([|null|]);")]
    [InlineData(Namespace.OrleanSpaces_Tuples_Specialized, "IntTemplate template = new([|null, null|]);")]
    [InlineData(Namespace.OrleanSpaces_Tuples_Specialized, "IntTemplate template = new([|null, null, null|]);")]
    public void Should_Diagnose(Namespace @namespace, string code) =>
        HasDiagnostic(code, @namespace);

    [Theory]

    [InlineData(Namespace.OrleanSpaces_Tuples, "SpaceTemplate template = new([|1|]);")]
    [InlineData(Namespace.OrleanSpaces_Tuples, "SpaceTemplate template = new([|1, null|]);")]
    [InlineData(Namespace.OrleanSpaces_Tuples, "SpaceTemplate template = new([|1, null, null|]);")]
    [InlineData(Namespace.OrleanSpaces_Tuples, "SpaceTemplate template = new([|null, 1|]);")]
    [InlineData(Namespace.OrleanSpaces_Tuples, "SpaceTemplate template = new([|null, 1, null|]);")]
    [InlineData(Namespace.OrleanSpaces_Tuples, "SpaceTemplate template = new([|null, 1, null, null|]);")]
    [InlineData(Namespace.OrleanSpaces_Tuples, "SpaceTemplate template = new([|null, 1, null, 1, null|]);")]

    [InlineData(Namespace.OrleanSpaces_Tuples_Specialized, "IntTemplate template = new([|1|]);")]
    [InlineData(Namespace.OrleanSpaces_Tuples_Specialized, "IntTemplate template = new([|1, null|]);")]
    [InlineData(Namespace.OrleanSpaces_Tuples_Specialized, "IntTemplate template = new([|1, null, null|]);")]
    [InlineData(Namespace.OrleanSpaces_Tuples_Specialized, "IntTemplate template = new([|null, 1|]);")]
    [InlineData(Namespace.OrleanSpaces_Tuples_Specialized, "IntTemplate template = new([|null, 1, null|]);")]
    [InlineData(Namespace.OrleanSpaces_Tuples_Specialized, "IntTemplate template = new([|null, 1, null, null|]);")]
    [InlineData(Namespace.OrleanSpaces_Tuples_Specialized, "IntTemplate template = new([|null, 1, null, 1, null|]);")]
    public void Should_Not_Diagnose(Namespace @namespace, string code) =>
        NoDiagnostic(code, @namespace);
}
