using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using System.Collections.Immutable;
using System.Composition;

namespace OrleanSpaces.Analyzers.OSA003;

/// <summary>
/// Code fix provider for <see cref="NotSupportedFieldTypeAnalyzer"/>.
/// </summary>
[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(NotSupportedFieldTypeFixer)), Shared]
internal sealed class NotSupportedFieldTypeFixer : CodeFixProvider
{
    public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(NotSupportedFieldTypeAnalyzer.Diagnostic.Id);
    public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken);
        if (root is null)
        {
            return;
        }

        var node = root.FindNode(context.Span);
        if (node is null)
        {
            return;
        }

        CodeAction action = CodeAction.Create(
            title: "Remove unsupported argument",
            equivalenceKey: NotSupportedFieldTypeAnalyzer.Diagnostic.Id,
            createChangedDocument: _ =>
            {
                var newRoot = root.RemoveNode(node, SyntaxRemoveOptions.KeepTrailingTrivia);

                return Task.FromResult(newRoot == null ? context.Document :
                    context.Document.WithSyntaxRoot(newRoot));
            });

        context.RegisterCodeFix(action, context.Diagnostics);
    }
}