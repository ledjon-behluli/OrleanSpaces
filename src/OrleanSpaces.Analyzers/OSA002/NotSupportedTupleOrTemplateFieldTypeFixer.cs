using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using System.Collections.Immutable;
using System.Composition;

namespace OrleanSpaces.Analyzers.OSA002;

/// <summary>
/// Code fix provider for <see cref="NotSupportedTupleOrTemplateFieldTypeAnalyzer"/>.
/// </summary>
[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(NotSupportedTupleOrTemplateFieldTypeFixer)), Shared]
internal sealed class NotSupportedTupleOrTemplateFieldTypeFixer : CodeFixProvider
{
    public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(NotSupportedTupleOrTemplateFieldTypeAnalyzer.Diagnostic.Id);
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
            title: "Remove argument.",
            equivalenceKey: NotSupportedTupleOrTemplateFieldTypeAnalyzer.Diagnostic.Id,
            createChangedDocument: _ =>
            {
                var newRoot = root.RemoveNode(node, SyntaxRemoveOptions.KeepNoTrivia);

                return Task.FromResult(newRoot == null ? context.Document :
                    context.Document.WithSyntaxRoot(newRoot));
            });

        context.RegisterCodeFix(action, context.Diagnostics);
    }
}