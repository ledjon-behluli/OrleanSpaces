using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;

namespace OrleanSpaces.Analyzers.OSA001;

/// <summary>
/// Reports to the client that interfaces marked with this attribute are intended for internal use only.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
internal class InternalUseOnlyAttributeAnalyzer : DiagnosticAnalyzer
{
    public static readonly DiagnosticDescriptor Diagnostic = new(
        id: "OSA001",
        category: Categories.Usage,
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        title: "Interface is intended for internal use only.",
        messageFormat: "Interface '{0}' is intended for internal use only.",
        helpLinkUri: "https://github.com/ledjon-behluli/OrleanSpaces/blob/master/docs/OrleanSpaces.Analyzers/Rules/OSA001.md");

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Diagnostic);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
        context.EnableConcurrentExecution();

        context.RegisterSyntaxNodeAction(AnalyzeIdentifierName, SyntaxKind.IdentifierName);
        context.RegisterSyntaxNodeAction(AnalyzeGenericName, SyntaxKind.GenericName);
    }

    private void AnalyzeIdentifierName(SyntaxNodeAnalysisContext context)
    {
        var identifierName = (IdentifierNameSyntax)context.Node;

        var symbol = context.SemanticModel.GetSymbolInfo(identifierName).Symbol;
        if (symbol is null)
        {
            return;
        }

        bool isInterfaceType = symbol is ITypeSymbol typeSymbol && typeSymbol.TypeKind == TypeKind.Interface;
        if (isInterfaceType && HasInternalUseOnlyAttribute(symbol))
        {
            ReportDiagnostic(in context, identifierName.GetLocation(), symbol.Name);
        }
    }

    private void AnalyzeGenericName(SyntaxNodeAnalysisContext context)
    {
        var genericName = (GenericNameSyntax)context.Node;

        var typeSymbol = context.SemanticModel.GetTypeInfo(genericName).Type;
        if (typeSymbol is null)
        {
            return;
        }

        foreach (var interfaceSymbol in typeSymbol.AllInterfaces)
        {
            if (HasInternalUseOnlyAttribute(interfaceSymbol))
            {
                ReportDiagnostic(in context, genericName.GetLocation(), $"{typeSymbol.Name}<T>");
                return;
            }
        }
    }

    private bool HasInternalUseOnlyAttribute(ISymbol symbol)
    {
        var attributeType = symbol.ContainingAssembly.GetTypeByMetadataName(FullyQualifiedNames.InternalUseOnlyAttribute);
        return symbol.GetAttributes().Any(x => SymbolEqualityComparer.Default.Equals(x.AttributeClass, attributeType));
    }

    private static void ReportDiagnostic(in SyntaxNodeAnalysisContext context, Location location, string symbolName)
        => context.ReportDiagnostic(Microsoft.CodeAnalysis.Diagnostic.Create(
               descriptor: Diagnostic,
               location: location,
               messageArgs: symbolName));
}