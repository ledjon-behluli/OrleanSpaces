using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;
using System.Collections.Immutable;

namespace OrleanSpaces.Analyzers.OSA001;

/// <summary>
/// Suggests to use existing reference value over initialization of a new one.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
internal sealed class TupleRefOverInitAnalyzer : DiagnosticAnalyzer
{
    public static readonly DiagnosticDescriptor Diagnostic = new(
        id: "OSA001",
        category: DiagnosticCategories.Performance,
        defaultSeverity: DiagnosticSeverity.Info,
        isEnabledByDefault: true,
        title: "Avoid instantiation by default constructor or expression.",
        messageFormat: "Avoid instantiation of '{0}' by default constructor or expression.");

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Diagnostic);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
        context.EnableConcurrentExecution();

        context.RegisterOperationAction(AnalyzeDefaultValue, OperationKind.DefaultValue);
        context.RegisterOperationAction(AnalyzeObjectCreation, OperationKind.ObjectCreation);

    }

    private void AnalyzeDefaultValue(OperationAnalysisContext context)
    {
        var operation = (IDefaultValueOperation)context.Operation;
        ReportDiagnostic(ref context, operation);
    }

    private void AnalyzeObjectCreation(OperationAnalysisContext context)
    {
        var operation = (IObjectCreationOperation)context.Operation;
        if (operation.Arguments.Length == 0)
        {
            ReportDiagnostic(ref context, operation);
        }
    }

    private static void ReportDiagnostic(ref OperationAnalysisContext context, IOperation operation)
    {
        if (AnalysisHelpers.IsAnyOfType(operation.Type, new List<ITypeSymbol?>()
        {
            context.Compilation.GetTypeByMetadataName(FullyQualifiedNames.SpaceUnit),
            context.Compilation.GetTypeByMetadataName(FullyQualifiedNames.SpaceTuple)
        }))
        {
            context.ReportDiagnostic(Microsoft.CodeAnalysis.Diagnostic.Create(
                descriptor: Diagnostic,
                location: operation.Syntax.GetLocation(),
                messageArgs: operation.Type?.Name,
                properties: ImmutableDictionary<string, string?>.Empty.Add("typeName", operation.Type?.Name)));
        }
    }
}
