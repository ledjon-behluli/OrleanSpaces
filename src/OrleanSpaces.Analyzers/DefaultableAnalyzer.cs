using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;
using System.Collections.Immutable;

namespace OrleanSpaces.Analyzers;

/// <summary>
/// Checks wether a type marked with a 'DefaultableAttribute' is being created via its default value.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
internal sealed class DefaultableAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "OSA001";

    public static readonly DiagnosticDescriptor Rule = new(
        id: DiagnosticId,
        category: DiagnosticCategory.Performance,
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        title: "Avoid using default constructor or value.",
        messageFormat: "To avoid unneccessary memory allocations, do not use the default constructor or default value.");

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        context.RegisterOperationAction(AnalyzeObjectCreation, OperationKind.ObjectCreation);
        context.RegisterOperationAction(AnalyzeDefaultValue, OperationKind.DefaultValue);
    }

    private void AnalyzeDefaultValue(OperationAnalysisContext context)
    {
        var operation = (IDefaultValueOperation)context.Operation;
        ReportDiagnosticForTypeIfNeeded(context.Compilation, operation.Syntax, operation.Type, Rule, context.ReportDiagnostic);
    }

    private void AnalyzeObjectCreation(OperationAnalysisContext context)
    {
        var operation = (IObjectCreationOperation)context.Operation;
        ReportDiagnosticForTypeIfNeeded(context.Compilation, operation.Syntax, operation.Type, Rule, context.ReportDiagnostic);
    }

    private static void ReportDiagnosticForTypeIfNeeded(
        Compilation compilation,
        SyntaxNode syntax,
        ITypeSymbol? type,
        DiagnosticDescriptor rule,
        Action<Diagnostic> reportDiagnostic)
    {
        if (type == null)
        {
            return;
        }

        if (DoNotUseDefaultConstruction(type, compilation, out var message))
        {
            message ??= string.Empty;

            if (!string.IsNullOrEmpty(message))
            {
                message = " " + message;
            }

            reportDiagnostic(Diagnostic.Create(rule, syntax.GetLocation(), type.Name, message));
        }

        static bool DoNotUseDefaultConstruction(ITypeSymbol type, Compilation compilation, out string? message)
        {
            message = null;

            var attributes = type.GetAttributes();
            var doNotUseDefaultAttribute = attributes.FirstOrDefault(a =>
            {
                if (a.AttributeClass is null)
                {
                    return false;
                }

                return a.AttributeClass.Name == "DefaultableAttribute";
            });

            if (doNotUseDefaultAttribute != null)
            {
                var constructorArg = doNotUseDefaultAttribute.ConstructorArguments.FirstOrDefault();
                message = !constructorArg.IsNull ? constructorArg.Value?.ToString() : null;

                return true;
            }

            return false;
        }
    }
}