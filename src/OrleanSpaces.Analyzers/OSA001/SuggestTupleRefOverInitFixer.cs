using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Immutable;
using System.Composition;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace OrleanSpaces.Analyzers.OSA001;

/// <summary>
/// Code fix provider for <see cref="SuggestTupleRefOverInitAnalyzer"/>.
/// </summary>
[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(SuggestTupleRefOverInitFixer)), Shared]
internal sealed class SuggestTupleRefOverInitFixer : CodeFixProvider
{
    public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(SuggestTupleRefOverInitAnalyzer.Diagnostic.Id);
    public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken);

        var node = root?.FindNode(context.Span);
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
            equivalenceKey: SuggestTupleRefOverInitAnalyzer.Diagnostic.Id,
            createChangedDocument: ct =>
            {
                var newNode = MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    IdentifierName(typeName),
                    IdentifierName("Null"));

                var newRoot = root?.ReplaceNode(node, newNode);

                return Task.FromResult(newRoot == null ? context.Document :
                    context.Document.WithSyntaxRoot(newRoot));
            });

        context.RegisterCodeFix(action, context.Diagnostics);
    }
}