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
        string newFixedCode = fixedCode;

        if (namespaces.Length > 0)
        {
            StringBuilder builder = new();

            foreach (var @namespace in namespaces)
            {
                builder.AppendLine($"using {@namespace.ToString().Replace('_', '.')};");
            }

            StringBuilder codeBuilder = new();
            StringBuilder fixedCodeBuilder = new();

            codeBuilder.Append(builder);
            codeBuilder.Append(code);

            fixedCodeBuilder.Append(builder);
            fixedCodeBuilder.Append(fixedCode);

            newCode = codeBuilder.ToString();
            newFixedCode = fixedCodeBuilder.ToString();
        }

        TestCodeFix(newCode, newFixedCode, diagnosticId);
    }
}