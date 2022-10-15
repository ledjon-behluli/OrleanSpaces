using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis;

namespace OrleanSpaces.Analyzers.Tests;

public class AnalyzerFixture : AnalyzerTestFixture
{
    private readonly string diagnosticId;
    private readonly DiagnosticAnalyzer analyzer;

    protected sealed override string LanguageName => LanguageNames.CSharp;
    protected sealed override DiagnosticAnalyzer CreateAnalyzer() => analyzer;

    protected sealed override IReadOnlyCollection<MetadataReference> References =>
        new[] { ReferenceSource.FromAssembly(typeof(ISpaceAgent).Assembly) };

    public AnalyzerFixture(DiagnosticAnalyzer analyzer, string diagnosticId)
    {
        this.analyzer = analyzer;
        this.diagnosticId = diagnosticId;
    }

    protected void NoDiagnostic(string source) => NoDiagnostic(source, diagnosticId);
    protected void HasDiagnostic(string source) => HasDiagnostic(source, diagnosticId);
}