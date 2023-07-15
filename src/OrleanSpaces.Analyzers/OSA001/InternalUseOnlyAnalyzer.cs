using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;

namespace OrleanSpaces.Analyzers.OSA001;

/// <summary>
/// Informs to the client that interfaces marked with this attribute are intended for internal use only.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
internal class InternalUseOnlyAttributeAnalyzer : DiagnosticAnalyzer
{
    public static readonly DiagnosticDescriptor Diagnostic = new(
        id: "OSA001",
        category: Categories.Usage,
        defaultSeverity: DiagnosticSeverity.Info,
        isEnabledByDefault: true,
        title: "Interface is intended for internal use only.",
        messageFormat: "Interface '{0}' is intended for internal use only.",
        helpLinkUri: "https://github.com/ledjon-behluli/OrleanSpaces/blob/master/docs/OrleanSpaces.Analyzers/Rules/OSA001.md");

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Diagnostic);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
        context.EnableConcurrentExecution();

        context.RegisterSyntaxNodeAction(AnalyzeInterfaceDeclaration, SyntaxKind.IdentifierName);
    }

    private void AnalyzeInterfaceDeclaration(SyntaxNodeAnalysisContext context)
    {
        var identifierName = (IdentifierNameSyntax)context.Node;
        var symbol = context.SemanticModel.GetSymbolInfo(identifierName).Symbol;

        if (symbol is null)
        {
            return;
        }

        if (IsInterfaceType(symbol) && HasInternalUseOnlyAttribute(symbol))
        {
            context.ReportDiagnostic(Microsoft.CodeAnalysis.Diagnostic.Create(
                    descriptor: Diagnostic,
                    location: identifierName.GetLocation(),
                    messageArgs: symbol.Name));
        }
    }

    private static bool IsInterfaceType(ISymbol symbol)
        => symbol is ITypeSymbol typeSymbol && typeSymbol.TypeKind == TypeKind.Interface;

    private bool HasInternalUseOnlyAttribute(ISymbol symbol)
    {
        var attributeType = symbol.ContainingAssembly.GetTypeByMetadataName(FullyQualifiedNames.InternalUseOnlyAttribute);
        return symbol.GetAttributes().Any(x => SymbolEqualityComparer.Default.Equals(x.AttributeClass, attributeType));
    }
}