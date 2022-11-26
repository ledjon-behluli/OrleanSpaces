using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;
using System.Collections.Immutable;

namespace OrleanSpaces.Analyzers.OSA002;

/// <summary>
/// Informs to create or use a 'SpaceTemplateCache' over initialization via 'new(...)', when all arguments are 'SpaceUnit's.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
internal sealed class SpaceTemplateCacheOverInitAnalyzer : DiagnosticAnalyzer
{
    public static readonly DiagnosticDescriptor Diagnostic = new(
        id: "OSA002",
        category: Categories.Performance,
        defaultSeverity: DiagnosticSeverity.Info,
        isEnabledByDefault: true,
        title: "Avoid constructor instantiation of 'SpaceTemplate' having only 'SpaceUnit' type arguments.",
        messageFormat: "Avoid constructor instantiation of 'SpaceTemplate' having only 'SpaceUnit' type arguments.",
        helpLinkUri: "https://github.com/ledjon-behluli/OrleanSpaces/blob/master/docs/OrleanSpaces.Analyzers/Rules/OSA002.md");

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Diagnostic);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
        context.EnableConcurrentExecution();

        context.RegisterOperationAction(AnalyzeObjectCreation, OperationKind.ObjectCreation);
    }

    private void AnalyzeObjectCreation(OperationAnalysisContext context)
    {
        var creationOperation = (IObjectCreationOperation)context.Operation;

        if (!creationOperation.Type.IsOfType(context.Compilation.GetTypeByMetadataName(FullyQualifiedNames.SpaceTemplate)))
        {
            return;
        }

        var spaceUnitSymbol = context.Compilation.GetTypeByMetadataName(FullyQualifiedNames.SpaceUnit);
        var arguments = creationOperation.GetArguments().ToList();

        if (arguments.Count == 0)
        {
            ReportDiagnostic(ref context, creationOperation, 1);
            return;
        }

        foreach (var argument in arguments)
        {
            var argumentType = creationOperation.SemanticModel?.GetTypeInfo(argument.Expression, context.CancellationToken).Type;
            if (!argumentType.IsOfType(spaceUnitSymbol))
            {
                return;
            }
        }

        ReportDiagnostic(ref context, creationOperation, arguments.Count);
    }

    private static void ReportDiagnostic(ref OperationAnalysisContext context, IOperation operation, int numOfSpaceUnits) =>
        context.ReportDiagnostic(Microsoft.CodeAnalysis.Diagnostic.Create(
            descriptor: Diagnostic,
            location: operation.Syntax.GetLocation(),
            properties: ImmutableDictionary<string, string?>.Empty.Add("numOfSpaceUnits", numOfSpaceUnits.ToString())));
}