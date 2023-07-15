using Microsoft.CodeAnalysis.Diagnostics;
using System.Text;

namespace OrleanSpaces.Analyzers.Tests;

public class AnalyzerFixture : AnalyzerTestFixture
{
    private readonly string diagnosticId;
    private readonly DiagnosticAnalyzer analyzer;

    protected sealed override string LanguageName => LanguageNames.CSharp;
    protected sealed override DiagnosticAnalyzer CreateAnalyzer() => analyzer;

    protected sealed override IReadOnlyCollection<MetadataReference> References => 
    new[]
    {
        ReferenceSource.FromAssembly(typeof(ISpaceAgent).Assembly),
        MetadataReference.CreateFromFile(Path.Combine(Path.GetDirectoryName(typeof(object).Assembly.Location), "System.Runtime.dll"))
    };

    public AnalyzerFixture(DiagnosticAnalyzer analyzer, string diagnosticId)
    {
        this.analyzer = analyzer;
        this.diagnosticId = diagnosticId;
    }

    protected void NoDiagnostic(string code, params Namespace[] namespaces)
    {
        string newCode = BuildCode(code, namespaces);
        NoDiagnostic(newCode, diagnosticId);
    }

    protected void HasDiagnostic(string code, params Namespace[] namespaces)
    {
        string newCode = BuildCode(code, namespaces);
        HasDiagnostic(newCode, diagnosticId);
    }

    private static string BuildCode(string code, params Namespace[] namespaces)
    {
        if (namespaces.Length == 0)
        {
            return code;
        }

        StringBuilder builder = new();

        foreach (var @namespace in namespaces)
        {
            builder.AppendLine($"using {@namespace.ToString().Replace('_', '.')};");
        }

        builder.Append(code);
        return builder.ToString();
    }
}