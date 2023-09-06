using OrleanSpaces.Analyzers.OSA004;

namespace OrleanSpaces.Analyzers.Tests.OSA004;

public class PreferSpecializedOverGenericAnalyzerTests : AnalyzerFixture
{
    public PreferSpecializedOverGenericAnalyzerTests() : base(
        new PreferSpecializedOverGenericAnalyzer(),
        PreferSpecializedOverGenericAnalyzer.Diagnostic.Id)
    {
        
    }

    [Fact]
    public void Should_Equal()
    {
        var diagnostic = PreferSpecializedOverGenericAnalyzer.Diagnostic;

        Assert.Equal("OSA004", diagnostic.Id);
        Assert.Equal(Categories.Performance, diagnostic.Category);
        Assert.Equal(DiagnosticSeverity.Warning, diagnostic.DefaultSeverity);
        Assert.Equal("Prefer using specialized over generic type.", diagnostic.Title);
        Assert.Equal("Prefer using specialized '{0}' over generic '{1}' type.", diagnostic.MessageFormat);
        Assert.True(diagnostic.IsEnabledByDefault);
    }

    [Fact]
    public void A()
    {
        string code = "SpaceTuple tuple = new([|1, 1, 1|]);";
        HasDiagnostic(code, Namespace.OrleanSpaces_Tuples);
    }
}
