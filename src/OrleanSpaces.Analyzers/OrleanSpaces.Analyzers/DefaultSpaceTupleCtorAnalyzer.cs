using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;

namespace OrleanSpaces.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
internal sealed class DefaultSpaceTupleCtorAnalyzer : DiagnosticAnalyzer
{
    public static readonly DiagnosticDescriptor Rule = DiagnosticDescriptors.DefaultSpaceTupleCtorDiagnostic;
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(ObjectCreationHandler, SyntaxKind.ObjectCreationExpression);
    }

    private void ObjectCreationHandler(SyntaxNodeAnalysisContext context)
    {
        ObjectCreationExpressionSyntax syntax = (ObjectCreationExpressionSyntax)context.Node;
        if (IsSpaceTupleType(context, syntax.Type) && syntax.ArgumentList.Arguments.Count == 0)
        {
            context.ReportDiagnostic(Diagnostic.Create(Rule, context.Node.GetLocation()));
        }
    }

    private bool IsSpaceTupleType(SyntaxNodeAnalysisContext context, TypeSyntax type)
    {
        SymbolInfo currentSymbolInfo = context.SemanticModel.GetSymbolInfo(type);
        INamedTypeSymbol targetSymbol = context.SemanticModel.Compilation.GetTypeByMetadataName("");

        return SymbolEqualityComparer.Default.Equals(currentSymbolInfo.Symbol, targetSymbol);
    }
}
