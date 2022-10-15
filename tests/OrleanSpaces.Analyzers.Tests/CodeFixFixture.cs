using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;

namespace OrleanSpaces.Analyzers.Tests;

public class CodeFixFixture : CodeFixTestFixture
{
    private readonly string diagnosticId;

    protected readonly DiagnosticAnalyzer analyzer;
    protected readonly CodeFixProvider provider;

    protected sealed override string LanguageName => LanguageNames.CSharp;
    protected sealed override CodeFixProvider CreateProvider() => provider;

    protected sealed override IReadOnlyCollection<DiagnosticAnalyzer> CreateAdditionalAnalyzers() => new[] { analyzer };
    protected sealed override IReadOnlyCollection<MetadataReference> References =>
        new[] { ReferenceSource.FromAssembly(typeof(ISpaceAgent).Assembly) };

    public CodeFixFixture(DiagnosticAnalyzer analyzer, CodeFixProvider provider, string diagnosticId)
    {
        this.analyzer = analyzer;
        this.provider = provider;
        this.diagnosticId = diagnosticId;
    }

    protected void TestCodeFix(string source, string fixedSource) => TestCodeFix(source, fixedSource, diagnosticId);
}