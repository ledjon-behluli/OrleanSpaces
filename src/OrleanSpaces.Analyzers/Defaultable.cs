using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;
using System.Collections.Immutable;
using System.Composition;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace OrleanSpaces.Analyzers;

/// <summary>
/// Checks wether a type marked with a 'DefaultableAttribute' is being created via its default value.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
internal sealed class DefaultableAnalyzer : DiagnosticAnalyzer
{
    public static readonly DiagnosticDescriptor Diagnostic = new(
        id: "OSA001",
        category: DiagnosticCategory.Performance,
        defaultSeverity: DiagnosticSeverity.Info,
        isEnabledByDefault: true,
        title: "Avoid instantiation by default constructor or expression.",
        messageFormat: "Avoid instantiation of '{0}' by default constructor or expression.");

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Diagnostic);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
        context.EnableConcurrentExecution();

        context.RegisterOperationAction(AnalyzeObjectCreation, OperationKind.ObjectCreation);
        context.RegisterOperationAction(AnalyzeDefaultValue, OperationKind.DefaultValue);
    }

    private void AnalyzeObjectCreation(OperationAnalysisContext context)
    {
        var operation = (IObjectCreationOperation)context.Operation;
        ReportDiagnostic(operation.Syntax, operation.Type, context.ReportDiagnostic);
    }

    private void AnalyzeDefaultValue(OperationAnalysisContext context)
    {
        var operation = (IDefaultValueOperation)context.Operation;
        ReportDiagnostic(operation.Syntax, operation.Type, context.ReportDiagnostic);
    }

    private static void ReportDiagnostic(SyntaxNode syntax, ITypeSymbol? type, Action<Diagnostic> action)
    {
        if (type?.GetAttributes().FirstOrDefault(a => a.AttributeClass?.Name == "DefaultableAttribute") != null)
        {
            action(Microsoft.CodeAnalysis.Diagnostic.Create(Diagnostic, syntax.GetLocation(), type.Name));
        }
    }
}

/// <summary>
/// Code fix provider for <see cref="DefaultableAnalyzer"/>.
/// </summary>
[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(DefaultableCodeFixProvider)), Shared]
internal sealed class DefaultableCodeFixProvider : CodeFixProvider
{
    private const string title = "Use 'SpaceTuple.Null'";

    public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(DefaultableAnalyzer.Diagnostic.Id);
    public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken);
        var node = root?.FindNode(context.Span);

        if (node == null)
        {
            return;
        }

        CodeAction action = CodeAction.Create(
            title: title,
            equivalenceKey: DefaultableAnalyzer.Diagnostic.Id,
            createChangedDocument: ct =>
            {
                var newNode = MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    IdentifierName("SpaceTuple"),
                    IdentifierName("Null"));

                var newRoot = root?.ReplaceNode(node, newNode);

                return Task.FromResult(newRoot == null ? context.Document :
                    context.Document.WithSyntaxRoot(newRoot));
            });

        context.RegisterCodeFix(action, context.Diagnostics);
    }
}