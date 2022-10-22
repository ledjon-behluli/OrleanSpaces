using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;
using System.Collections.Immutable;

namespace OrleanSpaces.Analyzers.OSA002;

/// <summary>
/// Warns that some of the arguments passed are not supported types.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
internal sealed class NotSupportedTupleFieldTypeAnalyzer : DiagnosticAnalyzer
{
    public static readonly DiagnosticDescriptor Diagnostic = new(
        id: "OSA002",
        category: Categories.Usage,
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        title: "The supplied argument type is not supported.",
        messageFormat: "The supplied argument '{0}' is not a supported '{1}' type.");

    private static readonly List<Type> simpleTypes = new()
    {
        // Primitives
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
        // Others
        typeof(Enum),
        typeof(string),
        typeof(decimal),
        typeof(DateTime),
        typeof(DateTimeOffset),
        typeof(TimeSpan),
        typeof(Guid)
    };

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

        // SpaceTuple
        if (operation.Type.IsOfType(context.Compilation.GetTypeByMetadataName(FullyQualifiedNames.SpaceTuple)))
        {
            foreach (var argument in operation.GetArguments())
            {
                var type = operation.SemanticModel?.GetTypeInfo(argument.Expression, context.CancellationToken).Type;

                if (type.IsOfAnyClrType(simpleTypes, context.Compilation))
                {
                    continue;
                }

                ReportDiagnosticFor("SpaceTuple", context, argument);
            }
        }

        // SpaceTemplate
        if (operation.Type.IsOfType(context.Compilation.GetTypeByMetadataName(FullyQualifiedNames.SpaceTemplate)))
        {
            var spaceUnitSymbol = context.Compilation.GetTypeByMetadataName(FullyQualifiedNames.SpaceUnit);

            foreach (var argument in operation.GetArguments())
            {
                var type = operation.SemanticModel?.GetTypeInfo(argument.Expression, context.CancellationToken).Type;

                if (type.IsOfAnyClrType(simpleTypes, context.Compilation) ||
                    type.IsOfClrType(typeof(Type), context.Compilation) ||
                    type.IsOfType(spaceUnitSymbol))
                {
                    continue;
                }

                ReportDiagnosticFor("SpaceTemplate", context, argument);
            }
        }

        static void ReportDiagnosticFor(string targetTypeName, OperationAnalysisContext context, ArgumentSyntax argument)
        {
            context.ReportDiagnostic(Microsoft.CodeAnalysis.Diagnostic.Create(
                descriptor: Diagnostic,
                location: argument.GetLocation(),
                messageArgs: new[] { argument.ToString(), targetTypeName }));
        }
    }
}