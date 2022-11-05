using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;

namespace OrleanSpaces.Analyzers.OSA004;

/// <summary>
/// Warns when a method which is supported only on full configuration, is called when partial configuration is used.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
internal sealed class InvalidMethodCallOnCurrentConfigAnalyzer : DiagnosticAnalyzer
{
    public static readonly DiagnosticDescriptor Diagnostic = new(
        id: "OSA004",
        category: Categories.Usage,
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        title: "Method is not supported on the configured environment.",
        messageFormat: "Method '{0}' is not supported on the configured environment.",
        helpLinkUri: "https://github.com/ledjon-behluli/OrleanSpaces/blob/master/docs/OrleanSpaces.Analyzers/Rules/OSA004.md");

    

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Diagnostic);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
        context.EnableConcurrentExecution();

        context.RegisterSyntaxNodeAction(AnalyseInvocation, SyntaxKind.InvocationExpression);
    }

    private static void AnalyseInvocation(SyntaxNodeAnalysisContext context)
    {
        var invocationExpression = (InvocationExpressionSyntax)context.Node;
        var memberAccessExpression = invocationExpression.Expression as MemberAccessExpressionSyntax;

        if (memberAccessExpression == null || !IsMethodOfInterest())
        {
            return;
        }

        ITypeSymbol? typeSymbol = null;

        // The target is a simple identifier, the code being analysed is of the form: "agent.PeekAsync(...)", and identifierName = "agent".
        if (memberAccessExpression.Expression is IdentifierNameSyntax identifierName)
        {
            typeSymbol = context.SemanticModel.GetTypeInfo(identifierName).Type;
        }

        // The target is another invocationSyntax, the code being analysed is of the form: "GetAgent().PeekAsync(...)", and _invocationExpression = "GetAgent()". 
        if (memberAccessExpression.Expression is InvocationExpressionSyntax _invocationExpression)
        {
            var symbol = context.SemanticModel.GetSymbolInfo(_invocationExpression).Symbol;
            typeSymbol = symbol is IMethodSymbol methodSymbol ? methodSymbol.ReturnType : null;
        }

        // The target is a member access, the code being analysed is of the form: "x.Agent.PeekAsync(...)", _memberAccessExpression = "x.Agent"
        if (memberAccessExpression.Expression is MemberAccessExpressionSyntax _memberAccessExpression)
        {
            var symbol = context.SemanticModel.GetSymbolInfo(_memberAccessExpression).Symbol;

            if (symbol is IFieldSymbol fieldSymbol) typeSymbol = fieldSymbol.Type;
            if (symbol is IPropertySymbol propertySymbol) typeSymbol = propertySymbol.Type;
        }

        if (typeSymbol?.Name == "ISpaceAgent")
        {
            context.ReportDiagnostic(Microsoft.CodeAnalysis.Diagnostic.Create(
               descriptor: Diagnostic,
               location: invocationExpression.GetLocation(),
               messageArgs: new[] { memberAccessExpression.Name.Identifier.ValueText }));
        }

        bool IsMethodOfInterest()
        {
            string methodName = memberAccessExpression.Name.Identifier.ValueText;

            if (methodName == "EvaluateAsync")
            {
                return true;
            }

            if (methodName == "PeekAsync" || methodName == "PopAsync")
            {
                // Arguments.Count = 2, because only the overloads with a callback delegate can not work without full configuration in place.
                if (invocationExpression.ChildNodes().OfType<ArgumentListSyntax>().Single().Arguments.Count == 2)
                {
                    return true;
                }
            }

            return false;
        }
    }
}