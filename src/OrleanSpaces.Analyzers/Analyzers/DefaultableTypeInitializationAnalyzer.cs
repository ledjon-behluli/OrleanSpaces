using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;
using System.Collections.Immutable;

namespace OrleanSpaces.Analyzers.Analyzers;

/// <summary>
/// Checks wether a type marked with a 'DefaultableAttribute' is being created via its default value.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
internal sealed class DefaultableTypeInitializationAnalyzer : DiagnosticAnalyzer
{
    public static readonly DiagnosticDescriptor Diagnostic = new(
        id: "OSA001",
        category: Categories.Performance,
        defaultSeverity: DiagnosticSeverity.Info,
        isEnabledByDefault: true,
        title: "Avoid instantiation by default constructor or expression.",
        messageFormat: "Avoid instantiation of '{0}' by default constructor or expression.");

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Diagnostic);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
        context.EnableConcurrentExecution();

        context.RegisterOperationAction(AnalyzeObjectCreation, OperationKind.ObjectCreation);
        context.RegisterOperationAction(AnalyzeDefaultValue, OperationKind.DefaultValue);
    }

    private void AnalyzeObjectCreation(OperationAnalysisContext context)
    {
        var operation = (IObjectCreationOperation)context.Operation;
        if (operation.Arguments.Length == 0)
        {
            ReportDiagnostic(operation.Syntax, operation.Type, context.ReportDiagnostic);
        }
    }

    private void AnalyzeDefaultValue(OperationAnalysisContext context)
    {
        var operation = (IDefaultValueOperation)context.Operation;
        ReportDiagnostic(operation.Syntax, operation.Type, context.ReportDiagnostic);
    }

    private static void ReportDiagnostic(SyntaxNode node, ITypeSymbol? type, Action<Diagnostic> action)
    {
        if (type?.GetAttributes().SingleOrDefault(a => a.AttributeClass?.Name == Constants.Defaultable_Attribute_Name) != null)
        {
            action(Microsoft.CodeAnalysis.Diagnostic.Create(
                descriptor: Diagnostic,
                location: node.GetLocation(),
                messageArgs: type.Name,
                properties: ImmutableDictionary<string, string?>.Empty.Add("typeName", type.Name)));
        }
    }
}
