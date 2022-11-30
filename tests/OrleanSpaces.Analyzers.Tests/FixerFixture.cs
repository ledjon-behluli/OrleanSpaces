using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using RoslynTestKit.CodeActionLocators;
using System.Text;
using System.Text.RegularExpressions;

namespace OrleanSpaces.Analyzers.Tests;

public class FixerFixture : CodeFixTestFixture
{
    private readonly string diagnosticId;

    protected readonly DiagnosticAnalyzer analyzer;
    protected readonly CodeFixProvider provider;

    protected sealed override string LanguageName => LanguageNames.CSharp;
    protected sealed override CodeFixProvider CreateProvider() => provider;

    protected sealed override IReadOnlyCollection<DiagnosticAnalyzer> CreateAdditionalAnalyzers() => new[] { analyzer };
    protected sealed override IReadOnlyCollection<MetadataReference> References =>
        new[] { ReferenceSource.FromAssembly(typeof(ISpaceAgent).Assembly) };

    public FixerFixture(DiagnosticAnalyzer analyzer, CodeFixProvider provider, string diagnosticId)
    {
        this.analyzer = analyzer;
        this.provider = provider;
        this.diagnosticId = diagnosticId;
    }

    /// <summary>
    /// Use for single actions.
    /// </summary>
    protected void TestCodeFix(string code, string fixedCode, params Namespace[] namespaces) =>
        TestCodeFix(code, fixedCode, namespaces, new ByIndexCodeActionSelector(0));

    /// <summary>
    /// Use for nested actions.
    /// </summary>
    protected void TestCodeFix(string groupTitle, string actionTitle, string code, string fixedCode, params Namespace[] namespaces) =>
        TestCodeFix(code, fixedCode, namespaces, new ByTitleForNestedActionSelector(groupTitle, actionTitle));

    private void TestCodeFix(string code, string fixedCode, Namespace[] namespaces, ICodeActionSelector selector)
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

            builder.AppendLine();

            StringBuilder codeBuilder = new();
            StringBuilder fixedCodeBuilder = new();

            codeBuilder.Append(builder);
            codeBuilder.Append(code);

            fixedCodeBuilder.Append(builder);
            fixedCodeBuilder.Append(fixedCode);

            newCode = codeBuilder.ToString();
            newFixedCode = fixedCodeBuilder.ToString();
        }

        //newCode = newCode;            // To ensure tests are cross-platform, due to line endings.
        newFixedCode = newFixedCode.ReplaceLineEndings();  // To ensure tests are cross-platform, due to line endings.

        TestCodeFix(newCode, newFixedCode, diagnosticId, selector);
    }

    private const string startTag = "[|";
    private const string endTag = "|]";
    private static readonly Regex regex = new(string.Format("{0}(.*?){1}", Regex.Escape(startTag), Regex.Escape(endTag)), RegexOptions.RightToLeft);

    /// <summary>
    /// Removes all text within the strings: '[|' and '|]'
    /// <i>
    /// <para>BEFORE: Test t = new Test([|1|]);</para>
    /// <para>AFTER: Test t = new Test();</para></i>
    /// </summary>
    protected static string RemoveTextWithinDiagnosticSpan(string code) => regex.Replace(code, "");
}