using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;
using System.Collections.Immutable;

namespace OrleanSpaces.Analyzers.OSA004;

/// <summary>
/// Reports to prefer specialized tuples & templates over the generic verisons.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
internal sealed class PreferSpecializedOverGenericAnalyzer : DiagnosticAnalyzer
{
    public static readonly DiagnosticDescriptor Diagnostic = new(
      id: "OSA004",
      category: Categories.Performance,
      defaultSeverity: DiagnosticSeverity.Warning,
      isEnabledByDefault: true,
      title: "Prefer using specialized over generic.",
      messageFormat: "Prefer using specialized '{0}' over generic '{1}'.",
      helpLinkUri: "https://github.com/ledjon-behluli/OrleanSpaces/blob/master/docs/OrleanSpaces.Analyzers/Rules/OSA004.md");

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

        if (creationOperation.Type.IsOfType(context.Compilation.GetTypeByMetadataName(FullyQualifiedNames.SpaceTuple)))
        {
            TryReportDiagnosticFor(in context, creationOperation, "SpaceTuple");
            return;
        }

        if (creationOperation.Type.IsOfType(context.Compilation.GetTypeByMetadataName(FullyQualifiedNames.SpaceTemplate)))
        {
            TryReportDiagnosticFor(in context, creationOperation, "SpaceTemplate");
            return;
        }
    }

    private static void TryReportDiagnosticFor(
        in OperationAnalysisContext context, IObjectCreationOperation creationOperation, string genericTypeName)
    {
        var arguments = creationOperation.GetArguments();
        if (arguments.Count() == 0)
        {
            return;
        }

        var argumentTypeSymbols = arguments.Select(arg => creationOperation.SemanticModel?.GetTypeInfo(arg.Expression).Type);
        if (argumentTypeSymbols == null)
        {
            return;
        }


        int sameTypeArgumentsCount = argumentTypeSymbols.Distinct(SymbolEqualityComparer.Default).Count();
        if (sameTypeArgumentsCount > 1)
        {
            return;
        }


        SpecialType? argumentType = argumentTypeSymbols.First()?.SpecialType;
        string specializedTypeName = argumentType switch
        {
            SpecialType.System_Boolean => "BoolTuple",
            // Continue
        };


        string argumentValues = string.Join(",", arguments
            .Select(arg => creationOperation.SemanticModel?.GetConstantValue(arg))
            .Select(val => val.HasValue ? val.Value.ToString() : string.Empty));

        context.ReportDiagnostic(Microsoft.CodeAnalysis.Diagnostic.Create(
            descriptor: Diagnostic,
            location: creationOperation.Syntax.GetLocation(),
            messageArgs: new[] { specializedTypeName, genericTypeName, argumentValues }));
    }

    private static readonly List<Type> simpleTypes = new()
    {
        typeof(bool),
        typeof(byte),
        typeof(sbyte),
        typeof(char),
        typeof(double),
        typeof(float),
        typeof(short),
        typeof(ushort),
        typeof(int),
        typeof(uint),
        typeof(long),
        typeof(ulong),
        typeof(Enum),
        typeof(string),
        typeof(decimal),
        typeof(DateTime),
        typeof(DateTimeOffset),
        typeof(TimeSpan),
        typeof(Guid)
    };

}
