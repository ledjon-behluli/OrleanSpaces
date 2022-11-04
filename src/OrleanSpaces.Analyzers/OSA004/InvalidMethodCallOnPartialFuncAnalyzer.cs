using Microsoft.CodeAnalysis.Diagnostics;

namespace OrleanSpaces.Analyzers.OSA004;

/// <summary>
/// Warns when a method which is supported only on full functionality, is called when partial functionality configuration is used.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
internal sealed class InvalidMethodCallOnPartialFuncAnalyzer : DiagnosticAnalyzer
{
    public static readonly DiagnosticDescriptor Diagnostic = new(
        id: "OSA002",
        category: Categories.Usage,
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        title: "The supplied argument type is not supported.",
        messageFormat: "The supplied argument '{0}' is not a supported '{1}' type.",
        helpLinkUri: "https://github.com/ledjon-behluli/OrleanSpaces/blob/master/docs/OrleanSpaces.Analyzers/Rules/OSA003.md");

}
