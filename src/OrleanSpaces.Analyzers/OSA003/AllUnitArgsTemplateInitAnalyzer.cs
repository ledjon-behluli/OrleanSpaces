using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;
using System.Collections.Immutable;

namespace OrleanSpaces.Analyzers.OSA003;

/// <summary>
/// Suggests to create reference SpaceTemplate over creation of all SpaceUnit type arguments.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
internal sealed class AllUnitArgsTemplateInitAnalyzer : DiagnosticAnalyzer
{
    public static readonly DiagnosticDescriptor Diagnostic = new(
        id: "OSA003",
        category: Categories.Performance,
        defaultSeverity: DiagnosticSeverity.Info,
        isEnabledByDefault: true,
        title: "Avoid instantiation of all 'SpaceUnit' type arguments.",
        messageFormat: "Avoid instantiation of all 'SpaceUnit' type arguments.");

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Diagnostic);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
        context.EnableConcurrentExecution();

        context.RegisterOperationAction(AnalyzeObjectCreation, OperationKind.ObjectCreation);
    }

    private void AnalyzeObjectCreation(OperationAnalysisContext context)
    {
        var operation = (IObjectCreationOperation)context.Operation;

        if (operation.Type.IsOfType(context.Compilation.GetTypeByMetadataName(FullyQualifiedNames.SpaceTemplate)))
        {
            var spaceUnitSymbol = context.Compilation.GetTypeByMetadataName(FullyQualifiedNames.SpaceUnit);
            var arguments = operation.GetArguments().ToList();

            if (arguments.Count == 0)
            {
                ReportDiagnostic(ref context, operation, 1);
            }

            int numOfSpaceUnits = 0;

            foreach (var argument in arguments)
            {
                var type = operation.SemanticModel?.GetTypeInfo(argument.Expression, context.CancellationToken).Type;
                if (type.IsOfType(spaceUnitSymbol))
                {
                    numOfSpaceUnits++;
                }
            }

            if (numOfSpaceUnits > 0)
            {
                ReportDiagnostic(ref context, operation, numOfSpaceUnits);
            }
        }
    }

    private static void ReportDiagnostic(ref OperationAnalysisContext context, IOperation operation, int numOfSpaceUnits) =>
        context.ReportDiagnostic(Microsoft.CodeAnalysis.Diagnostic.Create(
            descriptor: Diagnostic,
            location: operation.Syntax.GetLocation(),
            properties: ImmutableDictionary<string, string?>.Empty.Add("numOfSpaceUnits", numOfSpaceUnits.ToString())));
}