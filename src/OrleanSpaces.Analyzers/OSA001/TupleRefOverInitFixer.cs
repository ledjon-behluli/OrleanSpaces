using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using System.Collections.Immutable;
using System.Composition;

namespace OrleanSpaces.Analyzers.OSA001;

/// <summary>
/// Code fix provider for <see cref="TupleRefOverInitAnalyzer"/>.
/// </summary>
[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(TupleRefOverInitFixer)), Shared]
internal sealed class TupleRefOverInitFixer : CodeFixProvider
{
    public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(TupleRefOverInitAnalyzer.Diagnostic.Id);
    public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken);
        if (root == null)
        {
            return;
        }

        var node = root.FindNode(context.Span);
        if (node == null)
        {
            return;
        }

        string? typeName = context.Diagnostics.First().Properties.GetValueOrDefault("typeName");
        if (typeName == null)
        {
            return;
        }

        CodeAction action = CodeAction.Create(
            title: $"Use '{typeName}.Null'",
            equivalenceKey: TupleRefOverInitAnalyzer.Diagnostic.Id,
            createChangedDocument: _ =>
            {
                var newNode = MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    IdentifierName(typeName),
                    IdentifierName("Null"));

                var newRoot = root.ReplaceNode(node, newNode);

                return Task.FromResult(newRoot == null ? context.Document :
                    context.Document.WithSyntaxRoot(newRoot));
            });

        context.RegisterCodeFix(action, context.Diagnostics);
    }
}