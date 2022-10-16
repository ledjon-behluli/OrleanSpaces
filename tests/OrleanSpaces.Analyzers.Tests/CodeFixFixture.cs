using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Text;

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

    protected void TestCodeFix(string code, string fixedCode, params Namespace[] namespaces)
    {
        string newCode = code;

        if (namespaces.Length > 0)
        {
            StringBuilder builder = new();

            foreach (var @namespace in namespaces)
            {
                builder.AppendLine($"using {@namespace.ToString().Replace('_', '.')};");
            }

            builder.Append(code);
            newCode = builder.ToString();
        }

        TestCodeFix(newCode, fixedCode, diagnosticId);
    }
}