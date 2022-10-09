using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Immutable;
using System.Composition;

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
        title: "Avoid using default constructor/expression.",
        messageFormat: "To avoid unneccessary memory allocations, do not use default constructor/expression.");

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
        ReportDiagnosticForTypeIfNeeded(operation.Syntax, operation.Type, context.ReportDiagnostic);
    }

    private void AnalyzeObjectCreation(OperationAnalysisContext context)
    {
        var operation = (IObjectCreationOperation)context.Operation;
        ReportDiagnosticForTypeIfNeeded(operation.Syntax, operation.Type, context.ReportDiagnostic);
    }

    private static void ReportDiagnosticForTypeIfNeeded(SyntaxNode syntax, ITypeSymbol? type, Action<Diagnostic> reportDiagnostic)
    {
        if (type == null)
        {
            return;
        }

        if (type.GetAttributes().FirstOrDefault(a => a.AttributeClass?.Name == "DefaultableAttribute") != null)
        {
            reportDiagnostic(Diagnostic.Create(Rule, syntax.GetLocation(), type.Name));
        }
    }
}

/// <summary>
/// Provider for <see cref="DefaultableAnalyzer"/>.
/// </summary>
[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(DefaultableCodeFixProvider)), Shared]
internal sealed class DefaultableCodeFixProvider : CodeFixProvider
{
    public sealed override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(DefaultableAnalyzer.DiagnosticId);
    public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        SyntaxNode? root = await context.Document.GetSyntaxRootAsync(context.CancellationToken);
        if (root == null)
        {
            return;
        }

        Diagnostic diagnostic = context.Diagnostics.First();
        TextSpan span = diagnostic.Location.SourceSpan;

        ObjectCreationExpressionSyntax? syntax = root
            .FindToken(span.Start).Parent?
            .AncestorsAndSelf()
            .OfType<ObjectCreationExpressionSyntax>()
            .FirstOrDefault();

        if (syntax == null)
        {
            return;
        }

        CodeAction action = CodeAction.Create(
            title: "Use 'SpaceTuple.Null' instead.",
            equivalenceKey: DefaultableAnalyzer.DiagnosticId,
            createChangedDocument: ct => FixAsync(context.Document, syntax, ct));

        context.RegisterCodeFix(action, diagnostic);
    }

    private async Task<Document> FixAsync(Document document, ObjectCreationExpressionSyntax syntax, CancellationToken cancellationToken)
    {
        CompilationUnit()
            .WithMembers(
                SingletonList<MemberDeclarationSyntax>(
                    GlobalStatement(
                        LocalDeclarationStatement(
                            VariableDeclaration(
                                IdentifierName("SpaceTuple"))
                            .WithVariables(
                                SingletonSeparatedList<VariableDeclaratorSyntax>(
                                    VariableDeclarator(
                                        Identifier("tuple"))
                                    .WithInitializer(
                                        EqualsValueClause(
                                            MemberAccessExpression(
                                                SyntaxKind.SimpleMemberAccessExpression,
                                                IdentifierName("SpaceTuple"),
                                                IdentifierName("Null"))))))))))
            .NormalizeWhitespace();

        //return document.WithSyntaxRoot(root.ReplaceNode(syntax, newSyntax));
    }
}