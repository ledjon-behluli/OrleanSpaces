using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;
using System.Collections.Immutable;

namespace OrleanSpaces.Analyzers.OSA001;

/// <summary>
/// Informs to use the existing 'SpaceUnit.Null' reference, over initialization of 'new SpaceUnit()'.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
internal sealed class SpaceUnitRefOverInitAnalyzer : DiagnosticAnalyzer
{
    public static readonly DiagnosticDescriptor Diagnostic = new(
        id: "OSA001",
        category: Categories.Performance,
        defaultSeverity: DiagnosticSeverity.Info,
        isEnabledByDefault: true,
        title: "Avoid instantiation by default constructor or expression.",
        messageFormat: "Avoid instantiation of '{0}' by default constructor or expression.",
        helpLinkUri: "https://github.com/ledjon-behluli/OrleanSpaces");

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
        if (operation.Type.IsOfAnyType(new List<ITypeSymbol?>()
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
