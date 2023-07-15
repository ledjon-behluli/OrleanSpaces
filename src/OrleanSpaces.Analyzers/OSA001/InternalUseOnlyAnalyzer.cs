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

        context.RegisterSyntaxNodeAction(AnalyzeInterfaceDeclaration, SyntaxKind.InterfaceDeclaration);
    }

    private void AnalyzeInterfaceDeclaration(SyntaxNodeAnalysisContext context)
    {
        var interfaceDeclaration = (InterfaceDeclarationSyntax)context.Node;
        if (interfaceDeclaration is null)
        {
            return;
        }

        var interfaceSymbol = context.SemanticModel.GetDeclaredSymbol(interfaceDeclaration);
        if (interfaceSymbol is null)
        {
            return;
        }

        if (HasInternalUseOnlyAttribute(in context, interfaceSymbol) &&
            IsInterfaceUsedInDifferentAssembly(in context, interfaceDeclaration))
        {
            context.ReportDiagnostic(Microsoft.CodeAnalysis.Diagnostic.Create(
               descriptor: Diagnostic,
               location: interfaceDeclaration.Identifier.GetLocation(),
               messageArgs: interfaceSymbol.Name));
        }
    }

    private bool HasInternalUseOnlyAttribute(in SyntaxNodeAnalysisContext context, ISymbol symbol)
    {
        var attributeType = context.Compilation.GetTypeByMetadataName(FullyQualifiedNames.InternalUseOnlyAttribute);
        return symbol.GetAttributes().Any(x => SymbolEqualityComparer.Default.Equals(x.AttributeClass, attributeType));
    }

    private bool IsInterfaceUsedInDifferentAssembly(in SyntaxNodeAnalysisContext context, InterfaceDeclarationSyntax interfaceDeclaration)
        => !context.SemanticModel.Compilation.ContainsSyntaxTree(interfaceDeclaration.SyntaxTree);
}