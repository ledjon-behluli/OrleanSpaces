using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Composition;

namespace OrleanSpaces.Analyzers.OSA001;

/// <summary>
/// Code fix provider for <see cref="SpaceTupleNullRefOverInitAnalyzer"/>.
/// </summary>
[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(SpaceTupleNullRefOverInitFixer)), Shared]
internal sealed class SpaceTupleNullRefOverInitFixer : CodeFixProvider
{
    public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(SpaceTupleNullRefOverInitAnalyzer.Diagnostic.Id);
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

        CodeAction action = CodeAction.Create(
            title: "Use 'SpaceTuple.Null'",
            equivalenceKey: SpaceTupleNullRefOverInitAnalyzer.Diagnostic.Id,
            createChangedDocument: _ =>
            {
                SyntaxNode? newRoot = null;
                SyntaxKind nodeKind = node.Kind();

                switch (nodeKind)
                {
                    case SyntaxKind.ObjectCreationExpression:
                    case SyntaxKind.ImplicitObjectCreationExpression:
                    case SyntaxKind.DefaultExpression:
                    case SyntaxKind.DefaultLiteralExpression:
                        {
                            newRoot = root.ReplaceNode(node, CreateReplacmentSyntax());
                        }
                        break;
                    case SyntaxKind.Argument:
                        {
                            var objectCreationExpression = node.ChildNodes().OfType<ObjectCreationExpressionSyntax>().FirstOrDefault();
                            if (objectCreationExpression != null)
                            {
                                newRoot = root.ReplaceNode(node, Argument(CreateReplacmentSyntax()));
                            }
                        }
                        break;
                }

                return Task.FromResult(newRoot == null ? context.Document :
                    context.Document.WithSyntaxRoot(newRoot));
            });

        context.RegisterCodeFix(action, context.Diagnostics);
    }

    private static MemberAccessExpressionSyntax CreateReplacmentSyntax() =>
        MemberAccessExpression(
            SyntaxKind.SimpleMemberAccessExpression,
            IdentifierName("SpaceTuple"),
            IdentifierName("Null"));
}