using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;
using System.Collections.Immutable;

namespace OrleanSpaces.Analyzers.OSA002;

/// <summary>
/// Informs to create or use a '{X}TemplateCache' over initialization via 'new(...)', when all arguments are null's.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
internal sealed class TemplateCacheOverInitAnalyzer : DiagnosticAnalyzer
{
    public static readonly DiagnosticDescriptor Diagnostic = new(
        id: "OSA002",
        category: Categories.Performance,
        defaultSeverity: DiagnosticSeverity.Info,
        isEnabledByDefault: true,
        title: "Avoid constructor instantiation having only 'null' type, or no arguments.",
        messageFormat: "Avoid constructor instantiation of '{0}' having only 'null' type, or no arguments.",
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
       
        if (creationOperation.Type is null)
        {
            return;
        }

        if (!creationOperation.Type.IsOfAnyType(new()
        {
            context.Compilation.GetTypeByMetadataName(FullyQualifiedNames.SpaceTemplate),
            context.Compilation.GetTypeByMetadataName(FullyQualifiedNames.BoolTemplate),
            context.Compilation.GetTypeByMetadataName(FullyQualifiedNames.ByteTemplate),
            context.Compilation.GetTypeByMetadataName(FullyQualifiedNames.CharTemplate),
            context.Compilation.GetTypeByMetadataName(FullyQualifiedNames.DateTimeOffsetTemplate),
            context.Compilation.GetTypeByMetadataName(FullyQualifiedNames.DateTimeTemplate),
            context.Compilation.GetTypeByMetadataName(FullyQualifiedNames.DecimalTemplate),
            context.Compilation.GetTypeByMetadataName(FullyQualifiedNames.DoubleTemplate),
            context.Compilation.GetTypeByMetadataName(FullyQualifiedNames.FloatTemplate),
            context.Compilation.GetTypeByMetadataName(FullyQualifiedNames.GuidTemplate),
            context.Compilation.GetTypeByMetadataName(FullyQualifiedNames.HugeTemplate),
            context.Compilation.GetTypeByMetadataName(FullyQualifiedNames.IntTemplate),
            context.Compilation.GetTypeByMetadataName(FullyQualifiedNames.LongTemplate),
            context.Compilation.GetTypeByMetadataName(FullyQualifiedNames.SByteTemplate),
            context.Compilation.GetTypeByMetadataName(FullyQualifiedNames.ShortTemplate),
            context.Compilation.GetTypeByMetadataName(FullyQualifiedNames.TimeSpanTemplate),
            context.Compilation.GetTypeByMetadataName(FullyQualifiedNames.UHugeTemplate),
            context.Compilation.GetTypeByMetadataName(FullyQualifiedNames.UIntTemplate),
            context.Compilation.GetTypeByMetadataName(FullyQualifiedNames.ULongTemplate),
            context.Compilation.GetTypeByMetadataName(FullyQualifiedNames.UShortTemplate)
        }))
        {
            return;
        }

        var arguments = creationOperation.GetArguments().ToList();
        if (arguments.Count == 0)
        {
            ReportDiagnostic(in context, creationOperation, creationOperation.Type.Name, 1);
            return;
        }

        foreach (var argument in arguments)
        {
            var argumentType = creationOperation.SemanticModel?.GetTypeInfo(argument.Expression, context.CancellationToken).Type;
            if (argumentType is not null && argumentType.SpecialType != SpecialType.System_Nullable_T)
            {
                return;
            }
        }

        ReportDiagnostic(in context, creationOperation, creationOperation.Type.Name, arguments.Count);
    }

    private static void ReportDiagnostic(in OperationAnalysisContext context, IOperation operation, string templateTypeName, int numOfNulls)
    {
        var builder = ImmutableDictionary.CreateBuilder<string, string?>();
        
        builder.Add("TemplateTypeName", templateTypeName);
        builder.Add("NumOfNulls", numOfNulls.ToString());

        context.ReportDiagnostic(Microsoft.CodeAnalysis.Diagnostic.Create(
            descriptor: Diagnostic,
            location: operation.Syntax.GetLocation(),
            messageArgs: templateTypeName,
            properties: builder.ToImmutable()));
    }
}