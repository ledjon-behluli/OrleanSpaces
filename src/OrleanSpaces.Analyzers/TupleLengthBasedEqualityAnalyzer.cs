using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;
using System.Collections.Immutable;

namespace OrleanSpaces.Analyzers;

/// <summary>
/// Checks if the comparison between to tuples, is always 'false' based on different lengths.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
internal sealed class AlwaysFalseTupleEqualityAnalyzer : DiagnosticAnalyzer
{
    public static readonly DiagnosticDescriptor Diagnostic = new(
        id: "OSA002",
        category: DiagnosticCategory.Performance,
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        title: "The result of the expression is always 'false'.",
        messageFormat: "The result of the expression is always 'false' due to different 'Length' properties.");

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Diagnostic);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
        context.EnableConcurrentExecution();

        context.RegisterOperationAction(AnalyzeEqualsOperator, OperationKind.Binary);
        context.RegisterOperationAction(AnalyzeEqualsMethod, OperationKind.Invocation);
    }

    private void AnalyzeEqualsOperator(OperationAnalysisContext context)
    {
        var operation = (IBinaryOperation)context.Operation;
        if (operation.OperatorKind == BinaryOperatorKind.Equals)
        {
            var leftOperand = operation.LeftOperand;
            var rightOperand = operation.RightOperand;

            if (leftOperand.Type?.Name == rightOperand.Type?.Name && 
                leftOperand.Type?.ContainingNamespace.Name == "OrleanSpaces.Tuples")
            {

            }
        }
    }

    private void AnalyzeEqualsMethod(OperationAnalysisContext context)
    {
        
    }

    private static void ReportDiagnostic(SyntaxNode node, ITypeSymbol? type, Action<Diagnostic> action)
    {
        if (type?.GetAttributes().FirstOrDefault(a => a.AttributeClass?.Name == "DefaultableAttribute") != null)
        {
            action(Microsoft.CodeAnalysis.Diagnostic.Create(
                descriptor: Diagnostic,
                location: node.GetLocation(),
                messageArgs: type.Name,
                properties: ImmutableDictionary<string, string?>.Empty.Add("typeName", type.Name)));
        }
    }
}