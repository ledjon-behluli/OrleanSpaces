using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;
using System.Collections.Immutable;

namespace OrleanSpaces.Analyzers.OSA003;

/// <summary>
/// Reports when arguments passed to a 'SpaceTuple' or 'SpaceTemplate', are not supported field types.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
internal sealed class NotSupportedFieldTypeAnalyzer : DiagnosticAnalyzer
{
    private const string spaceTupleName = "SpaceTuple";
    private const string spaceTemplateName = "SpaceTemplate";

    // Int128 & UInt128 are not available in netstandard2.0, so they are not part of the list of types
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

    public static readonly DiagnosticDescriptor Diagnostic = new(
        id: "OSA003",
        category: Categories.Usage,
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        title: "The supplied argument type is not supported.",
        messageFormat: "The supplied argument '{0}' is not a supported '{1}' type.",
        helpLinkUri: "https://github.com/ledjon-behluli/OrleanSpaces/blob/master/docs/OrleanSpaces.Analyzers/Rules/OSA003.md");

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
            TryReportDiagnosticFor(in context, creationOperation, spaceTupleName);
            return;
        }

        if (creationOperation.Type.IsOfType(context.Compilation.GetTypeByMetadataName(FullyQualifiedNames.SpaceTemplate)))
        {
            TryReportDiagnosticFor(in context, creationOperation, spaceTemplateName);
            return;
        }
    }

    private static void TryReportDiagnosticFor(
        in OperationAnalysisContext context, IObjectCreationOperation creationOperation, string targetTypeName)
    {
        foreach (var argument in creationOperation.GetArguments())
        {
            var type = creationOperation.SemanticModel?.GetTypeInfo(argument.Expression, context.CancellationToken).Type;

            if (targetTypeName == spaceTemplateName)
            {
                if (type is null || type.SpecialType == SpecialType.System_Nullable_T)
                {
                    continue;
                }
            }

            if (type.IsOfAnyClrType(simpleTypes, context.Compilation) ||
                type.IsOfAnyType(new List<ITypeSymbol?>()
                {
                    context.Compilation.GetTypeByMetadataName("System.Int128"),
                    context.Compilation.GetTypeByMetadataName("System.UInt128"),
                    context.Compilation.GetTypeByMetadataName("System.Object[]")
                }))
            {
                continue;
            }

            context.ReportDiagnostic(Microsoft.CodeAnalysis.Diagnostic.Create(
                descriptor: Diagnostic,
                location: argument.GetLocation(),
                messageArgs: new[] { argument.ToString(), targetTypeName }));
        }
    }
}