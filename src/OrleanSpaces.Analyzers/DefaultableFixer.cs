using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Immutable;
using System.Composition;

namespace OrleanSpaces.Analyzers;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(DefaultCtorFixer)), Shared]
internal sealed class DefaultCtorFixer : CodeFixProvider
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

        ObjectCreationExpressionSyntax syntax = root
            .FindToken(span.Start).Parent
            .AncestorsAndSelf()
            .OfType<ObjectCreationExpressionSyntax>()
            .First();

        CodeAction action = CodeAction.Create(
            title: "Use 'SpaceTuple.Null' instead.",
            equivalenceKey: DefaultableAnalyzer.DiagnosticId,
            createChangedDocument: ct => FixAsync(context.Document, syntax, ct));

        context.RegisterCodeFix(action, diagnostic);
    }

    private async Task<Document> FixAsync(Document document, ObjectCreationExpressionSyntax syntax, CancellationToken cancellationToken)
    {
        return null;
    }
}