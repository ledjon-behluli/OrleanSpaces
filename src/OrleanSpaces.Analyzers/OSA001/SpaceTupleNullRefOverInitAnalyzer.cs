using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;
using System.Collections.Immutable;

namespace OrleanSpaces.Analyzers.OSA001;

/// <summary>
/// Informs to use the existing 'SpaceTuple.Null' reference, over initialization via 'new()'.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
internal sealed class SpaceTupleNullRefOverInitAnalyzer : DiagnosticAnalyzer
{
    public static readonly DiagnosticDescriptor Diagnostic = new(
        id: "OSA001",
        category: Categories.Performance,
        defaultSeverity: DiagnosticSeverity.Info,
        isEnabledByDefault: true,
        title: "Avoid instantiation of empty 'SpaceTuple' by default constructor or expression.",
        messageFormat: "Avoid instantiation of empty 'SpaceTuple' by default constructor or expression.",
        helpLinkUri: "https://github.com/ledjon-behluli/OrleanSpaces/blob/master/docs/OrleanSpaces.Analyzers/Rules/OSA001.md");

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
        var defaultValueOperation = (IDefaultValueOperation)context.Operation;
        ReportDiagnostic(ref context, defaultValueOperation);
    }

    private void AnalyzeObjectCreation(OperationAnalysisContext context)
    {
        var creationOperation = (IObjectCreationOperation)context.Operation;
        if (creationOperation.Arguments.Length == 0)
        {
            ReportDiagnostic(ref context, creationOperation);
        }
    }

    private static void ReportDiagnostic(ref OperationAnalysisContext context, IOperation operation)
    {
        if (operation.Type.IsOfType(context.Compilation.GetTypeByMetadataName(FullyQualifiedNames.SpaceTuple)))
        {
            context.ReportDiagnostic(Microsoft.CodeAnalysis.Diagnostic.Create(
                descriptor: Diagnostic,
                location: operation.Syntax.GetLocation()));
        }
    }
}
